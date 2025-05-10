using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;

namespace ResumeGenerator.AuthService.Application.Services;

public interface IAuthService
{
    RegisterUserResponse RegisterUser(RegisterUserRequest request);
    LoginUserResponse LoginUser(LoginUserRequest request);
}