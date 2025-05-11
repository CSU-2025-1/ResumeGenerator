using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.AuthService.Data.Context;

namespace ResumeGenerator.AuthService.Web.Initializers;

public class DatabaseInitializer : IAsyncInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            await context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migration applied successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}
