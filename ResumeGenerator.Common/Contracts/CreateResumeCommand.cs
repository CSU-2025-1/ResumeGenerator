namespace ResumeGenerator.Common.Contracts;

public sealed record CreateResumeCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string DesiredPosition,
    string GitHubLink,
    string TelegramLink,
    string Email,
    string PhoneNumber,
    string Education,
    string Experience,
    string[] HardSkills,
    string[] SoftSkills
);