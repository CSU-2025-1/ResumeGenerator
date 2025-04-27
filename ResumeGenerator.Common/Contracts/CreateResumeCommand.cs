namespace ResumeGenerator.Common.Contracts;

public sealed record CreateResumeCommand(
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