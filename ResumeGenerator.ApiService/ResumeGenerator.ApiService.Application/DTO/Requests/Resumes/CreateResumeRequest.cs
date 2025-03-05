namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public record CreateResumeRequest
{
    public required ResumeDto Resume { get; init; }
}