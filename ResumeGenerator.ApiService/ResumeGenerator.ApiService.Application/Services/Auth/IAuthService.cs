namespace ResumeGenerator.ApiService.AuthClientGrpc;

public interface IAuthService
{
    Task<Guid> GetUserIdFromTokenAsync(string token, CancellationToken ct = default);
}