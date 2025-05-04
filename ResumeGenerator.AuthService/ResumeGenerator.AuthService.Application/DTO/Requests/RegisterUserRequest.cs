namespace ResumeGenerator.AuthService.Application.DTO.Requests;

public record RegisterUserRequest(
    string Username,
    string Password
);