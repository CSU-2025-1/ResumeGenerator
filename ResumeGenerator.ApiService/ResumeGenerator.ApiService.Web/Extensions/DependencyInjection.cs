using Microsoft.OpenApi.Models;

namespace ResumeGenerator.ApiService.Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ResumeAppMVP", Version = "v1" });
        });
}