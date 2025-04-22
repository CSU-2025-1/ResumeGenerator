using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Data.Context;

namespace ResumeGenerator.ApiService.Web.Initializers;

public sealed class MigrationAsyncInitializer : IAsyncInitializer
{
    private readonly AppDbContext _context;

    public MigrationAsyncInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if ((await _context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await _context.Database.MigrateAsync(cancellationToken);
        }
    }
}