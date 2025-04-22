namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public sealed record GetResumesByUserIdRequest
{
    public required Guid UserId { get; init; }
}