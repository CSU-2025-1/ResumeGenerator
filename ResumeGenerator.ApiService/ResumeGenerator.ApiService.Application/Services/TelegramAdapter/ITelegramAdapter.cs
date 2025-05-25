using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.TelegramAdapter;

public interface ITelegramAdapter
{
    Task SendResumeToUserAsync(Resume resume, CancellationToken ct = default);
}