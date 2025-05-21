using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.ApiService.Data.Entities;

public sealed class Resume
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ResumeName { get; init; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserPatronymic { get; set; }
    public string DesiredPosition { get; set; }
    public string GitHubLink { get; set; }
    public string TelegramLink { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Education { get; set; }
    public int ExperienceYears { get; set; }

    public ResumeStatus ResumeStatus { get; set; } = ResumeStatus.ResumeMakingInProgress;
    public int RetryCount { get; set; }

    public ICollection<HardSkill> HardSkills { get; set; } = [];
    public ICollection<SoftSkill> SoftSkills { get; set; } = [];
}