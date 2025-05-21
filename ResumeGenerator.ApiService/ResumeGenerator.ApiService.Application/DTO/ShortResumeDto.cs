using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.DTO;

public sealed record ShortResumeDto
{
    public Guid Id { get; init; }
    public string ResumeName { get; init; }
    
    public ResumeStatus ResumeStatus { get; init; } = ResumeStatus.ResumeMakingInProgress;
}