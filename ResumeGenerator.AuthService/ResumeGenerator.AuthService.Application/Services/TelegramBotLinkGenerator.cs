using Microsoft.Extensions.Options;
using ResumeGenerator.AuthService.Application.Configuration;

namespace ResumeGenerator.AuthService.Application.Services;

public sealed class TelegramBotLinkGenerator : IBotLinkGenerator
{
    private readonly TelegramBotOptions _options;

    public TelegramBotLinkGenerator(IOptions<TelegramBotOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateLink(string guidUserId)
    {
        return _options.DeepLinkTemplate
            ?? $"https://t.me/{_options.Username}?start={guidUserId}";
    }
}