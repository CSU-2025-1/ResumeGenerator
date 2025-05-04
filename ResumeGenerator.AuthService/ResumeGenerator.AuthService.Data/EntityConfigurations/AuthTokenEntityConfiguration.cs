using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Data.EntityConfigurations;

public sealed class AuthTokenEntityConfiguration : IEntityTypeConfiguration<AuthToken>
{
    public void Configure(EntityTypeBuilder<AuthToken> builder)
    {
        builder.ToTable("auth_tokens");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id).HasColumnName("id");
        builder.Property(h => h.UserId).HasColumnName("user_id");
        builder.Property(h => h.Token).HasColumnName("token");
        builder.Property(h => h.CreatedAt).HasColumnName("created_at");
        builder.Property(h => h.ExpiresAt).HasColumnName("expires_at");
    }
}