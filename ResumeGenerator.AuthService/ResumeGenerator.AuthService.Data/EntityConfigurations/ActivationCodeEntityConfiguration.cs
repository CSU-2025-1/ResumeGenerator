using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Data.EntityConfigurations;

public sealed class ActivationCodeEntityConfiguration : IEntityTypeConfiguration<ActivationCode>
{
    public void Configure(EntityTypeBuilder<ActivationCode> builder)
    {
        builder.ToTable("activation_codes");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id).HasColumnName("id");
        builder.Property(h => h.UserId).HasColumnName("user_id");
        builder.Property(h => h.Code).HasColumnName("code");
        builder.Property(h => h.IsUsed).HasColumnName("is_used");
        builder.Property(h => h.CreatedAt).HasColumnName("created_at");
        builder.Property(h => h.ExpiresAt).HasColumnName("expires_at");
    }
}