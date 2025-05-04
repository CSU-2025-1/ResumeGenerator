namespace ResumeGenerator.AuthService.Application.DTO.Responses;

public record RegisterUserResponse(
    Guid UserId,
    string BotActivationLink
);