using Microsoft.EntityFrameworkCore;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Data.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<User> Users => Set<User>();
    public DbSet<AuthToken> AuthTokens => Set<AuthToken>();
    public DbSet<ActivationCode> ActivationCodes => Set<ActivationCode>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}