using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;
using ResumeGenerator.AuthService.Application.Exceptions;
using ResumeGenerator.AuthService.Data.Context;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IBotLinkGenerator _botLinkGenerator;

    public AuthService(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        IBotLinkGenerator botLinkGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _botLinkGenerator = botLinkGenerator;
    }

    public RegisterUserResponse RegisterUser(RegisterUserRequest request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
        {
            throw new UserAlreadyExistsException(request.Username);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return new RegisterUserResponse(
            user.Id
        );
    }

    public LoginUserResponse LoginUser(LoginUserRequest request)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        { 
            throw new UserNotFoundException(request.Username); 
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        { 
            throw new InvalidCredentialsException(); 
        }

        if (!user.IsActive)
        {
            throw new UserNotActiveException();
        }

        var token = _tokenGenerator.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddDays(7);

        //var authToken = new AuthToken
        //{
        //    Id = Guid.NewGuid(),
        //    UserId = user.Id,
        //    Token = token,
        //    CreatedAt = DateTime.UtcNow,
        //    ExpiresAt = expiresAt
        //};

        //_context.AuthTokens.Add(authToken);
        _context.SaveChanges();

        return new LoginUserResponse(token);
    }
}