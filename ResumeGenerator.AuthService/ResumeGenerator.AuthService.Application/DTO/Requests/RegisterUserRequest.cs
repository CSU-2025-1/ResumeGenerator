namespace ResumeGenerator.AuthService.Application.DTO.Requests;

public sealed record RegisterUserRequest(
    string Username,
    string Password
);