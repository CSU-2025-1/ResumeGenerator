namespace ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;

public record GetResumesByUserIdResponse
{
    public required ResumeDto[] Resumes { get; init; }
}