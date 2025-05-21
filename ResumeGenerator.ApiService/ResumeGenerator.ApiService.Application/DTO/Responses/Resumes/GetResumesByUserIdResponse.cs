namespace ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;

public sealed record GetResumesByUserIdResponse
{
    public required ShortResumeDto[] Resumes { get; init; }
}