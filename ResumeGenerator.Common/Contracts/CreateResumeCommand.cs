namespace ResumeGenerator.Common.Contracts;

public sealed record CreateResumeCommand
{
    public required Guid ResumeId { get; init; }
    public required Guid UserId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string MiddleName { get; init; }
    public required string DesiredPosition { get; init; }
    public required string GitHubLink { get; init; }
    public required string TelegramLink { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Education { get; init; }
    public required int ExperienceYears { get; init; }
    public required string[] HardSkills { get; init; }
    public required string[] SoftSkills { get; init; }
}