using CSharpFunctionalExtensions;
using ResumeGenerator.TelegramAdapter.Core.Entities;

namespace ResumeGenerator.TelegramAdapter.Core.Abstractions;

public interface ITelegramChatRepository
{
    Task<Maybe<TelegramChat>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Result> SaveChatAsync(TelegramChat chat, CancellationToken ct = default);
}