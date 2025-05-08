using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Grpc.Generated;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ResumeGenerator.TelegramAdapter.Grpc.Services;

public sealed class TelegramAdapterService : Generated.TelegramAdapter.TelegramAdapterBase
{
    private readonly ITelegramChatRepository _telegramChatRepository;
    private readonly IResumeRepository _resumeRepository;
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramAdapterService(
        ITelegramChatRepository telegramChatRepository,
        IResumeRepository resumeRepository,
        ITelegramBotClient telegramBotClient)
    {
        _telegramChatRepository = telegramChatRepository;
        _resumeRepository = resumeRepository;
        _telegramBotClient = telegramBotClient;
    }

    public override async Task<Empty> SendResume(SendResumeRequest request, ServerCallContext context)
    {
        var userId = ParseGuid(request.UserID, nameof(request.UserID));
        var resumeId = ParseGuid(request.ResumeID, nameof(request.ResumeID));

        var chat = await _telegramChatRepository.GetByUserIdAsync(userId, context.CancellationToken);
        if (chat.HasNoValue)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Telegram chat for that user wasn't found"));
        }

        var resumeMs = await _resumeRepository.GetResumeByIdAsync(resumeId, ct: context.CancellationToken);
        if (resumeMs.HasNoValue)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Resume not found"));
        }

        await _telegramBotClient.SendDocument(
            chatId: chat.Value.ExtId,
            document: InputFile.FromStream(resumeMs.Value),
            cancellationToken: context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SendResumeFailedMessage(
        SendResumeFailedMessageRequest request,
        ServerCallContext context)
    {
        var userId = ParseGuid(request.UserID, nameof(request.UserID));

        var chat = await _telegramChatRepository.GetByUserIdAsync(userId, context.CancellationToken);
        if (chat.HasNoValue)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Telegram chat for that user wasn't found"));
        }

        await _telegramBotClient.SendMessage(
            chatId: chat.Value.ExtId,
            text: "Нам не удалось сгенерировать одно из резюме, которые вы отправляли. Пожалуйста, повторите попытку," +
                  " либо же свяжитесь с поддержкой",
            cancellationToken: context.CancellationToken);

        return new Empty();
    }

    private static Guid ParseGuid(string guid, string paramName)
    {
        if (!Guid.TryParse(guid, out var parsedGuid))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"{paramName} is invalid"));
        }

        return parsedGuid;
    }
}