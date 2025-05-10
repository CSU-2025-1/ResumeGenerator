namespace ResumeGenerator.AuthService.Application.Configuration;

public sealed class TelegramBotOptions
{
    public const string SectionName = "TelegramBot";

    public string Username { get; set; } = string.Empty;
    public string? DeepLinkTemplate { get; set; }
}