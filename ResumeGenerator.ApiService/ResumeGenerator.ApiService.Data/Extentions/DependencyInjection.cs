using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeGenerator.ApiService.Data.Context;

namespace ResumeGenerator.ApiService.Data.Extentions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}