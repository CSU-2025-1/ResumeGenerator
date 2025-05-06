using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.Entities;
using ResumeGenerator.TelegramAdapter.Core.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ResumeGenerator.TelegramAdapter.Core.UpdateHandlers;

public sealed class StartCommandUpdateHandler : IUpdateHandler
{
    private readonly ITelegramChatRepository _telegramChatRepository;

    public StartCommandUpdateHandler(ITelegramChatRepository telegramChatRepository)
    {
        _telegramChatRepository = telegramChatRepository;
    }

    public ValueTask<bool> CanHandleAsync(Update update, CancellationToken ct = default)
        => ValueTask.FromResult(
            update.Type is UpdateType.Message &&
            update.Message!.Text is not null &&
            update.Message.Text.StartsWith("/start ", StringComparison.OrdinalIgnoreCase)
        );

    public async Task HandleAsync(Update update, CancellationToken ct = default)
    {
        string[] messageTextParts = update.Message!.Text!.Split(' ');
        if (messageTextParts.Length != 2)
        {
            throw new ActivationCodeMissingException();
        }

        string activationCodeString = messageTextParts[1];
        if (!Guid.TryParse(activationCodeString, out var activationCode))
        {
            throw new InvalidActivationCodeException();
        }

        // TODO отправить запрос в auth-service для активации пользователя
        await _telegramChatRepository.SaveChatAsync(new TelegramChat
        {
            ExtId = update.Message.Chat.Id,
            UserId = activationCode
        }, ct);
    }
}