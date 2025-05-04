namespace ResumeGenerator.AuthService.Application.DTO.Responses;

public record LoginUserResponse(
    string Token,
    DateTime ExpiresAt
);