namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public sealed record GetResumesByUserIdRequest
{
    public required Guid UserId { get; init; }
    public required int PageNumber { get; init; } = 1;
    public required int PageSize { get; init; } = 10;
}