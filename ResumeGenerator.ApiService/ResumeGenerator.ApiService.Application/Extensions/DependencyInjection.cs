using Microsoft.Extensions.DependencyInjection;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Handlers.Resumes;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Dto;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Extensions;

public static class DependencyInjection
{
     public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        //Resume handlers
        services.AddScoped<CreateResumeHandler>();
        services.AddScoped<GetAllResumesByUserIdHandler>();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IResumeService, ResumeService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        //Resume validators
        services.AddScoped<CreateResumeRequestValidator>();
        services.AddScoped<GetResumesByUserIdRequestValidator>();
        
        //Dto validators
        services.AddScoped<ResumeDtoValidator>();
        
        return services;
    }
}