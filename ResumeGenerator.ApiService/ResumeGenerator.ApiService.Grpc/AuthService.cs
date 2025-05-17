using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.Application.Services.Auth;
using ResumeGenerator.ApiService.Grpc.protos;

namespace ResumeGenerator.ApiService.Grpc;

public sealed class AuthService : IAuthService
{
    private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;

    public AuthService(AuthServiceGrpc.AuthServiceGrpcClient client)
    {
        _client = client;
    }

    public async Task<Guid> GetUserIdFromTokenAsync(string token, CancellationToken ct = default)
    {
        var response = await _client.GetUserByTokenAsync(
            new GetUserByTokenRequest { Token = token }, cancellationToken: ct).ResponseAsync;

        if (!response.IsActive)
        {
            UnauthorizedException.ThrowWithError(new Error(StatusCodes.Status401Unauthorized.ToString(),
                "User is not active"));
        }

        return Guid.Parse(response.Id);
    }
}