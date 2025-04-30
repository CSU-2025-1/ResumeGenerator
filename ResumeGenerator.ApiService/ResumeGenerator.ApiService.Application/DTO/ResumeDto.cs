using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.DTO;

public sealed record ResumeDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; } //Костыль на время пока Аутха нет
    public string UserFirstName { get; init; }
    public string UserLastName { get; init; }
    public string UserPatronymic { get; init; }
    public string DesiredPosition { get; init; }
    public string GitHubLink { get; init; }
    public string TelegramLink { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string Education { get; init; }
    public int ExperienceYears { get; init; }
    public string HardSkills { get; init; }
    public string SoftSkills { get; init; }
    
    public ResumeStatus ResumeStatus { get; init; } = ResumeStatus.ResumeMakingInProgress;
}