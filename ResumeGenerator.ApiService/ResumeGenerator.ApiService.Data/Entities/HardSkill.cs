namespace ResumeGenerator.ApiService.Data.Entities;

public sealed class HardSkill
{
    public Guid Id { get; set; }
    public string HardSkillName { get; set; }

    public Guid ResumeId { get; set; }
    public Resume? Resume { get; set; }
}