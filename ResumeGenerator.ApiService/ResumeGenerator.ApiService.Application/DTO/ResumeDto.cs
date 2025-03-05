namespace ResumeGenerator.ApiService.Application.DTO;

public record ResumeDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } //Костыль на время пока Аутха нет
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
    public string HardSkills { get; set; }
    public string SoftSkills { get; set; }
}