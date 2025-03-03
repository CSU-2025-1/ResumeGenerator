using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.ApiService.Data.Entities;

public class Resume
{
    public required Guid ResumeId { get; set; }
    
    [MaxLength(50)]
    public required string UserFirstName { get; set; }
    
    [MaxLength(50)]
    public required string UserLastName { get; set; }
    
    [MaxLength(50)]
    public required string UserPatronymic { get; set; }
    
    [MaxLength(20)]
    public required string DesiredPosition { get; set; }
    
    public required string GitHubLink { get; set; }
    
    [MaxLength(50)]
    public required string TelegramLink { get; set; }
    
    public required string Email { get; set; }
    
    [MaxLength(11)]
    public required string PhoneNumber { get; set; }
    
    [MaxLength(80)]
    public required string Education { get; set; }
    
    public required int ExperienceYears { get; set; }
    
    public required string HardSkills { get; set; }
    public required string SoftSkills { get; set; }
}