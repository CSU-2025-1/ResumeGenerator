using Minio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Minio.Extensions;

public static class DependencyInjection
{
    public static void AddMinio(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(section["Endpoint"])
            .WithCredentials(section["AccessKey"], section["SecretKey"])
            .WithSSL(false)
            .Build());
        services.AddScoped<IResumeRepository, ResumeRepository>();
    }
}