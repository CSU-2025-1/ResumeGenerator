using DbUp;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Persistence;

public sealed class Migrator
{
    private readonly string? _connectionString;

    public Migrator(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public void Migrate()
    {
        EnsureDatabase.For.PostgresqlDatabase(_connectionString);

        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(GetType().Assembly)
            .Build();

        if (upgradeEngine.IsUpgradeRequired())
        {
            upgradeEngine.PerformUpgrade();
        }
    }
}