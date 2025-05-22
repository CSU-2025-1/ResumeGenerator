using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.Services.TelegramAdapter;
using ResumeGenerator.ApiService.Data.Entities;
using ResumeGenerator.ApiService.Grpc.protos;

namespace ResumeGenerator.ApiService.Grpc;

public sealed class TelegramAdapterService : ITelegramAdapter
{
    private readonly TelegramAdapter.TelegramAdapterClient _telegramAdapterClient;
    private readonly ILogger<TelegramAdapterService> _logger;

    public TelegramAdapterService(
        TelegramAdapter.TelegramAdapterClient telegramAdapterClient,
        ILogger<TelegramAdapterService> logger)
    {
        _telegramAdapterClient = telegramAdapterClient;
        _logger = logger;
    }

    public async Task SendResumeToUserAsync(Resume resume, CancellationToken ct = default)
    {
        try
        {
            await _telegramAdapterClient.SendResumeAsync(new SendResumeRequest
            {
                ResumeID = resume.Id.ToString(),
                ResumeTitle = resume.ResumeName,
                UserID = resume.UserId.ToString()
            }, cancellationToken: ct).ResponseAsync;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error resending resume to user");
        }
    }
}