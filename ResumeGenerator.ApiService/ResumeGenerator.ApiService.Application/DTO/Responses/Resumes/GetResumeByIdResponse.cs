namespace ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;

public sealed record GetResumeByIdResponse
{
    public required ResumeDto Resume { get; init; }
}