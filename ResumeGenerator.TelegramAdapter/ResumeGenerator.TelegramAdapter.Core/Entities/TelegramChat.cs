namespace ResumeGenerator.TelegramAdapter.Core.Entities;

public sealed class TelegramChat
{
    public Guid UserId { get; init; }
    public long ExtId { get; init; }
}