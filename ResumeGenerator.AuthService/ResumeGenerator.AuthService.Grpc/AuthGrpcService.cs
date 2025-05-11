using System.Threading.Tasks;
using Grpc.Core;
using ResumeGenerator.AuthService.Application.Services;
using ResumeGenerator.AuthService.Grpc;

namespace ResumeGenerator.AuthService.Grpc;

public sealed class AuthGrpcService : AuthServiceGrpc.AuthServiceGrpcBase
{
    private readonly IAuthService _authService;
    
    public AuthGrpcService(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<ActivateUserResponse> ActivateUser(
        ActivateUserRequest request,
        ServerCallContext context)
    {
        var result = await _authService.ActivateUserAsync(request.ActivationCode, context.CancellationToken);
        return new ActivateUserResponse
        {
            Success = result.Success,
            Message = result.Message
        };
    }

    public override async Task<GetUserByTokenResponse> GetUserByToken(
        GetUserByTokenRequest request,
        ServerCallContext context)
    {
        var user = await _authService.GetUserByTokenAsync(request.Token, context.CancellationToken);
        return new GetUserByTokenResponse
        {
            Id = user.Id.ToString(),
            Username = user.Username,
            IsActive = user.IsActive
        };
    }
}
