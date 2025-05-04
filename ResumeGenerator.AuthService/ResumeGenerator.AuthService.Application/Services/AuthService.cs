using System.Data.Entity;
using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;
using ResumeGenerator.AuthService.Application.Exceptions;
using ResumeGenerator.AuthService.Data.Context;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IActivationCodeGenerator _activationCodeGenerator;
    private readonly IBotLinkGenerator _botLinkGenerator;

    public AuthService(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        IActivationCodeGenerator activationCodeGenerator,
        IBotLinkGenerator botLinkGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _activationCodeGenerator = activationCodeGenerator;
        _botLinkGenerator = botLinkGenerator;
    }

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            throw new UserAlreadyExistsException(request.Username);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            ChatId = null,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var activationCode = new ActivationCode
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Code = _activationCodeGenerator.GenerateCode(),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        await _context.Users.AddAsync(user);
        await _context.ActivationCodes.AddAsync(activationCode);
        await _context.SaveChangesAsync();

        return new RegisterUserResponse(
            user.Id,
            _botLinkGenerator.GenerateLink(activationCode.Code)
        );
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            throw new UserNotFoundException(request.Username);

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        if (!user.IsActive)
            throw new UserNotActiveException();

        var token = _tokenGenerator.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var authToken = new AuthToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        };

        await _context.AuthTokens.AddAsync(authToken);
        await _context.SaveChangesAsync();

        return new LoginUserResponse(token, expiresAt);
    }
}