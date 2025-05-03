using CSharpFunctionalExtensions;

namespace ResumeGenerator.TelegramAdapter.Core.Abstractions;

public interface IResumeRepository
{
    Task<Maybe<MemoryStream>> GetResumeByIdAsync(Guid id, CancellationToken ct = default);
}