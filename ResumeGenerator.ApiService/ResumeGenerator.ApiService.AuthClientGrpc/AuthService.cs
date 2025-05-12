using ResumeGenerator.AuthService.Grpc;

namespace ResumeGenerator.ApiService.AuthClientGrpc;

public sealed class AuthService : IAuthService
{
    private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;

    public AuthService(AuthServiceGrpc.AuthServiceGrpcClient client)
    {
        _client = client;
    }

    public async Task<Guid> GetUserIdFromTokenAsync(string token, CancellationToken ct = default)
    {
        var response =
            await _client.GetUserByTokenAsync(new GetUserByTokenRequest { Token = token }, cancellationToken: ct);

        if (!response.IsActive)
        {
            throw new UnauthorizedAccessException("User is not active");
        }

        return Guid.Parse(response.Id);
    }
}