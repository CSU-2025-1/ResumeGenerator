using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Data.EntityConfigurations;

public sealed class ResumeEntityConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable("resumes");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.UserId).HasColumnName("user_id");
        builder.Property(u => u.ResumeName).HasColumnName("resume_name");
        builder.Property(u => u.UserFirstName).HasColumnName("user_first_name");
        builder.Property(u => u.UserLastName).HasColumnName("user_last_name");
        builder.Property(u => u.UserPatronymic).HasColumnName("user_patronymic");
        builder.Property(u => u.DesiredPosition).HasColumnName("desired_position");
        builder.Property(u => u.GitHubLink).HasColumnName("github_link");
        builder.Property(u => u.TelegramLink).HasColumnName("telegram_link");
        builder.Property(u => u.Email).HasColumnName("email");
        builder.Property(u => u.PhoneNumber).HasColumnName("phone_number");
        builder.Property(u => u.Education).HasColumnName("education");
        builder.Property(u => u.ExperienceYears).HasColumnName("experience_years");
        builder.Property(u => u.ResumeStatus).HasColumnName("resume_status");
        builder.Property(r => r.RetryCount).HasColumnName("retry_count");
        
        builder.HasMany(r => r.HardSkills)
            .WithOne(hs => hs.Resume)
            .HasForeignKey(hs => hs.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.SoftSkills)
            .WithOne(ss => ss.Resume)
            .HasForeignKey(ss => ss.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}