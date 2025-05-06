using Telegram.Bot.Types;

namespace ResumeGenerator.TelegramAdapter.Core.Abstractions;

public interface IUpdateHandler
{
    ValueTask<bool> CanHandleAsync(Update update, CancellationToken ct = default);
    Task HandleAsync(Update update, CancellationToken ct = default);
}