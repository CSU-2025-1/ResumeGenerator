using System.Text;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ResumeGenerator.AuthService.Data.Context;

namespace ResumeGenerator.AuthService.Grpc.Services;

public class AuthGrpcService : AuthGrpcService.AuthGrpcServiceBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthGrpcService> _logger;

    public AuthGrpcService(AppDbContext context, ILogger<AuthGrpcService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<ActivateUserResponse> ActivateUser(
        ActivateUserRequest request,
        ServerCallContext context)
    {
        try
        {
            var activationCode = await _context.ActivationCodes
                .Include(ac => ac.User)
                .FirstOrDefaultAsync(ac =>
                    ac.Code == request.ActivationCode &&
                    !ac.IsUsed);

            if (activationCode == null || activationCode.ExpiresAt < DateTime.UtcNow)
            {
                return new ActivateUserResponse
                {
                    Success = false,
                    Message = "Неверный или просроченный код активации"
                };
            }

            // Активируем пользователя
            activationCode.IsUsed = true;
            activationCode.User.IsActive = true;
            await _context.SaveChangesAsync();

            return new ActivateUserResponse
            {
                Success = true,
                Message = "Аккаунт успешно активирован"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка активации пользователя");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<GetUserByTokenResponse> GetUserByToken(
        GetUserByTokenRequest request,
        ServerCallContext context)
    {
        var userId = ValidateTokenAndGetUserId(request.Token);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, "Пользователь не найден"));
        }

        return new GetUserByTokenResponse
        {
            Id = user.Id.ToString(),
            Username = user.Username,
            ChatId = user.ChatId ?? 0,
            IsActive = user.IsActive
        };
    }

    private Guid ValidateTokenAndGetUserId(string token)
    {
        // Реализуйте валидацию JWT (как в вашем AuthService)
        // Пример:
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("your_jwt_secret_key");

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        return Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
    }
}