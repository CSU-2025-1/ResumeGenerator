namespace ResumeGenerator.ApiService.Application.Services.Auth;

public interface IAuthService
{
    Task<Guid> GetUserIdFromTokenAsync(string token, CancellationToken ct = default);
}