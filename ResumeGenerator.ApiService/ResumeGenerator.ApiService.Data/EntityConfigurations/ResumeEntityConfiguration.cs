using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Data.EntityConfigurations;

public class ResumeEntityConfiguration: IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.ResumeId);
        builder.HasAlternateKey(u => u.PhoneNumber);
    }
}