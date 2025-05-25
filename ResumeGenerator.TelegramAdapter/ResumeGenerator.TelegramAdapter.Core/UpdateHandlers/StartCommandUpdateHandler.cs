using Grpc.Core;
using Microsoft.Extensions.Logging;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.Entities;
using ResumeGenerator.TelegramAdapter.Core.Exceptions;
using ResumeGenerator.TelegramAdapter.Grpc.Clients.Generated;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ResumeGenerator.TelegramAdapter.Core.UpdateHandlers;

public sealed class StartCommandUpdateHandler : IUpdateHandler
{
    private readonly ITelegramChatRepository _telegramChatRepository;
    private readonly AuthServiceGrpc.AuthServiceGrpcClient _authServiceGrpcClient;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<StartCommandUpdateHandler> _logger;

    public StartCommandUpdateHandler(
        ITelegramChatRepository telegramChatRepository,
        AuthServiceGrpc.AuthServiceGrpcClient authServiceGrpcClient,
        ITelegramBotClient telegramBotClient,
        ILogger<StartCommandUpdateHandler> logger)
    {
        _telegramChatRepository = telegramChatRepository;
        _authServiceGrpcClient = authServiceGrpcClient;
        _telegramBotClient = telegramBotClient;
        _logger = logger;
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

        await ActivateUserAsync(activationCodeString, ct);

        await _telegramChatRepository.SaveChatAsync(new TelegramChat
        {
            ExtId = update.Message.Chat.Id,
            UserId = activationCode
        }, ct);

        await _telegramBotClient.SendMessage(
            chatId: update.Message.Chat.Id,
            text: "Поздравляем с успешной активацией аккаунта! Теперь вы можете продолжить работу на сайте",
            cancellationToken: ct);
    }

    private async Task ActivateUserAsync(string activationCode, CancellationToken ct = default)
    {
        try
        {
            await _authServiceGrpcClient.ActivateUserAsync(new ActivateUserRequest
            {
                ActivationCode = activationCode
            }, cancellationToken: ct).ResponseAsync;
        }
        catch (RpcException e)
        {
            _logger.LogError(e, "Failed to activate user with status-code {Code}", e.StatusCode);
        }
    }
}