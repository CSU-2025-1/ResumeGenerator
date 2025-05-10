using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Data.EntityConfigurations;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id).HasColumnName("id");
        builder.Property(h => h.Username).HasColumnName("username").IsRequired();
        builder.Property(h => h.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(h => h.IsActive).HasColumnName("is_active").HasDefaultValue(false);
        builder.Property(h => h.CreatedAt).HasColumnName("created_at");
        builder.Property(h => h.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(u => u.Username).IsUnique();
    }
}