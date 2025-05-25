namespace ResumeGenerator.AuthService.Application.DTO.Responses;

public sealed record UserDto(Guid Id, string Username, bool IsActive);