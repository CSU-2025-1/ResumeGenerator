using System.Data;
using Npgsql;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Persistence;

public sealed class DbConnectionFactory
{
    private readonly string? _connectionString;

    public DbConnectionFactory(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> ConnectAsync(CancellationToken ct = default)
    {
        var con = new NpgsqlConnection(_connectionString);
        await con.OpenAsync(ct);
        return con;
    }
}