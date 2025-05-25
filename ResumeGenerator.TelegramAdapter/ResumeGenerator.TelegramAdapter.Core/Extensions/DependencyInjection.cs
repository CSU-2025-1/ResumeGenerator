using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ResumeGenerator.TelegramAdapter.Core.Extensions;

public static class DependencyInjection
{
    public static void AddTelegramBotClient(
        this IServiceCollection services,
        ITelegramBotClient telegramBotClient)
    => services.Add(
        ServiceDescriptor.Describe(
            serviceType: typeof(ITelegramBotClient),
            implementationFactory: _ => telegramBotClient,
            lifetime: ServiceLifetime.Singleton)
    );
}