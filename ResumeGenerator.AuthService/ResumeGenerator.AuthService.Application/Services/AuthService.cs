using System.Threading;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;
using ResumeGenerator.AuthService.Application.Exceptions;
using ResumeGenerator.AuthService.Data.Context;
using ResumeGenerator.AuthService.Data.Entities;
using System.Threading.Tasks;
using System.Security.Claims;
using static ResumeGenerator.AuthService.Application.Services.IAuthService;

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

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken ct = default)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username, ct))
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

        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        return new RegisterUserResponse(user.Id);
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request, CancellationToken ct = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, ct);

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
        return new LoginUserResponse(token);
    }

    public async Task<ActivationResult> ActivateUserAsync(string activationCode, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == activationCode, ct);

        if (user == null)
        {
            return new ActivationResult(false, "Пользователь не найден");
        }

        if (user.IsActive)
        {
            return new ActivationResult(true, "Пользователь уже активирован");
        }

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return new ActivationResult(true, "Аккаунт успешно активирован");
    }

    public async Task<UserDto> GetUserByTokenAsync(string token, CancellationToken ct = default)
    {
        var principal = _tokenGenerator.ValidateToken(token);
        var userId = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var user = await _context.Users.FindAsync(new object[] { Guid.Parse(userId) }, ct);

        if (user == null)
            throw new InvalidOperationException("Пользователь не найден");

        return new UserDto(user.Id, user.Username, user.IsActive);
    }
}