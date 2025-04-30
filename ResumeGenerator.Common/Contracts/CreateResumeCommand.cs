namespace ResumeGenerator.Common.Contracts;

public sealed class CreateResumeCommand()
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
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

    public CreateResumeCommand(Guid resumeId,
        Guid userId,
        string firstName,
        string lastName,
        string middleName,
        string desiredPosition,
        string gitHubLink,
        string telegramLink,
        string email,
        string phoneNumber,
        string education,
        int experienceYears,
        string[] hardSkills,
        string[] softSkills) : this()
    {
    }
}