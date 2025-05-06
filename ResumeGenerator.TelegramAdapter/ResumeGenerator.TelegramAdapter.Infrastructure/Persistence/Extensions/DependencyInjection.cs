using Microsoft.Extensions.DependencyInjection;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Persistence.Extensions;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, string? conString)
    {
        services.Add(
            ServiceDescriptor.Describe(
                serviceType: typeof(DbConnectionFactory),
                implementationFactory: _ => new DbConnectionFactory(conString),
                lifetime: ServiceLifetime.Singleton)
        );
        services.AddScoped<ITelegramChatRepository, TelegramChatRepository>();
    }
}