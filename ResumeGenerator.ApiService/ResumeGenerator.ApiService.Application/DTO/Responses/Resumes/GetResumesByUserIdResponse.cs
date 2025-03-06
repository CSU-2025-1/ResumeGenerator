namespace ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;

public  sealed  record GetResumesByUserIdResponse
{
    public required ResumeDto[] Resumes { get; init; }
}