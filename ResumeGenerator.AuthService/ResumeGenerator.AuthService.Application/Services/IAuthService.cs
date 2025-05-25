using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;

namespace ResumeGenerator.AuthService.Application.Services;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken ct = default);
    Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request, CancellationToken ct = default);
    Task ActivateUserAsync(Guid activationCode, CancellationToken ct = default);
    Task<UserDto> GetUserByTokenAsync(string token, CancellationToken ct = default);
}