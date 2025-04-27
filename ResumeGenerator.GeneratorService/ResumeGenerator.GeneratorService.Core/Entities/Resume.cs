namespace ResumeGenerator.GeneratorService.Core.Entities;

public readonly record struct Resume(
    Guid ResumeId,
    Guid UserId,
    string FirstName,
    string LastName,
    string MiddleName,
    string DesiredPosition,
    string GitHubLink,
    string TelegramLink,
    string Email,
    string PhoneNumber,
    string Education,
    int ExperienceYears,
    string[] HardSkills,
    string[] SoftSkills
);
