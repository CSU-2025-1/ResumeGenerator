using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Data.EntityConfigurations;

public sealed class SoftSkillEntityConfiguration : IEntityTypeConfiguration<SoftSkill>
{
    public void Configure(EntityTypeBuilder<SoftSkill> builder)
    {
        builder.ToTable("soft_skills");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.SoftSkillName).HasColumnName("soft_skill_name");
        builder.Property(s => s.ResumeId).HasColumnName("resume_id");
    }
}