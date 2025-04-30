using Microsoft.Extensions.DependencyInjection;
using ResumeGenerator.ApiService.Application.Handlers.Resumes;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Dto;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
        => services.AddScoped<CreateResumeHandler>()
            .AddScoped<GetAllResumesByUserIdHandler>()
            .AddScoped<GetResumeByIdHandler>()
            .AddScoped<DeleteResumeByIdHandler>();

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services.AddScoped<IResumeService, ResumeService>();

    public static IServiceCollection AddValidators(this IServiceCollection services)
        => services.AddScoped<CreateResumeRequestValidator>()
            .AddScoped<GetResumesByUserIdRequestValidator>()
            .AddScoped<GetResumeByIdRequestValidator>()
            .AddScoped<DeleteResumeByIdRequestValidator>()
            .AddScoped<ResumeDtoValidator>();
}