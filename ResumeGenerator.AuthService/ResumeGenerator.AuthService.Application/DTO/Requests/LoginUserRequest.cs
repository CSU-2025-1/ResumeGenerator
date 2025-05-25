namespace ResumeGenerator.AuthService.Application.DTO.Requests;

public sealed record LoginUserRequest(
    string Username,
    string Password
);