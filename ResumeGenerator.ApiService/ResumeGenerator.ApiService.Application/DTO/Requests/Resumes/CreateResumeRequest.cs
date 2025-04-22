namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public sealed record CreateResumeRequest
{
    public required ResumeDto Resume { get; init; }
}