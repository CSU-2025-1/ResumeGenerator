namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public sealed record GetResumeByIdRequest
{
    public required Guid ResumeId { get; init; }
    public required Guid UserId { get; init; }
}