using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Data.EntityConfigurations;

public sealed class HardSkillEntityConfiguration : IEntityTypeConfiguration<HardSkill>
{
    public void Configure(EntityTypeBuilder<HardSkill> builder)
    {
        builder.ToTable("hard_skills");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id).HasColumnName("id");
        builder.Property(h => h.HardSkillName).HasColumnName("hard_skill_name");
        builder.Property(h => h.ResumeId).HasColumnName("resume_id");
    }
}