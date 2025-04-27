namespace ResumeGenerator.ApiService.Data.Entities;

public sealed class SoftSkill
{
    public Guid Id { get; set; }
    public string SoftSkillName { get; set; }

    public Guid ResumeId { get; set; }
    public Resume? Resume { get; set; }
}