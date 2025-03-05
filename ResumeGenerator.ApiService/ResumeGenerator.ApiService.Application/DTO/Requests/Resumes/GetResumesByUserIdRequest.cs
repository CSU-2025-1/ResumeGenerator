namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public record GetResumesByUserIdRequest
{
    public required Guid UserId { get; init; }
}