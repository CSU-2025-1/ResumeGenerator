using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Data.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Resume> Resumes => Set<Resume>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}