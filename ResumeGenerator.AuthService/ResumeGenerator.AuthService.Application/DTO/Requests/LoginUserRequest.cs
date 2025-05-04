namespace ResumeGenerator.AuthService.Application.DTO.Requests;

public record LoginUserRequest(
    string Username,
    string Password
);