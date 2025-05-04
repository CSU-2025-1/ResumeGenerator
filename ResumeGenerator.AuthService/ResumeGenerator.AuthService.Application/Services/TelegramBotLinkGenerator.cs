using Microsoft.Extensions.Configuration;

namespace ResumeGenerator.AuthService.Application.Services;

public class TelegramBotLinkGenerator : IBotLinkGenerator
{
    private readonly IConfiguration _configuration;

    public TelegramBotLinkGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateLink(string code)
    {
        var botUsername = _configuration["TelegramBot:Username"];
        return $"https://t.me/{botUsername}?start={code}";
    }
}