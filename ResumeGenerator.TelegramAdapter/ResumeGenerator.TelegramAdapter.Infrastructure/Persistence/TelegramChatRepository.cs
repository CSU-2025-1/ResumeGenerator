using CSharpFunctionalExtensions;
using Dapper;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.Entities;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Persistence;

public sealed class TelegramChatRepository : ITelegramChatRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public TelegramChatRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Maybe<TelegramChat>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
            const string query = $"""
                                  SELECT user_id AS {nameof(TelegramChat.UserId)},
                                         ext_id AS {nameof(TelegramChat.ExtId)}
                                  FROM telegram_chat 
                                  WHERE user_id = @{nameof(userId)};
                                  """;
            using var con = await _connectionFactory.ConnectAsync(ct);

            var chat = await con.QueryFirstOrDefaultAsync<TelegramChat>(query, new { userId });

            return Maybe<TelegramChat>.From(chat);
    }

    public async Task<Result> SaveChatAsync(TelegramChat chat, CancellationToken ct = default)
    {
        try
        {
            const string command = $"""
                                    INSERT INTO telegram_chat(user_id, ext_id)
                                    VALUES (@{nameof(chat.UserId)}, @{nameof(chat.ExtId)});
                                    """;
            using var con = await _connectionFactory.ConnectAsync(ct);

            await con.ExecuteAsync(command, chat);

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}