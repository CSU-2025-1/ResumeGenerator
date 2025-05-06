using Microsoft.Extensions.Logging;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using Telegram.Bot.Types;

namespace ResumeGenerator.TelegramAdapter.Core;

public sealed class UpdateDispatcher
{
    private readonly IEnumerable<IUpdateHandler> _updateHandlers;
    private readonly ILogger<UpdateDispatcher> _logger;

    public UpdateDispatcher(IEnumerable<IUpdateHandler> updateHandlers, ILogger<UpdateDispatcher> logger)
    {
        _updateHandlers = updateHandlers;
        _logger = logger;
    }

    public async Task DispatchAsync(Update update, CancellationToken ct = default)
    {
        try
        {
            foreach (var handler in _updateHandlers)
            {
                if (!await handler.CanHandleAsync(update, ct))
                {
                    continue;
                }

                await handler.HandleAsync(update, ct);
                break;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while dispatching update {UpdateID}", update.Id);
        }
    }
}