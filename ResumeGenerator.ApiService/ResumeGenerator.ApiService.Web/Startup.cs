using ResumeGenerator.ApiService.Application.Extensions;
using ResumeGenerator.ApiService.Application.Mapping;
using ResumeGenerator.ApiService.Data.Extentions;
using ResumeGenerator.ApiService.Web.Extensions;
using ResumeGenerator.ApiService.Web.Initializers;
using ResumeGenerator.ApiService.Web.Middlewares;
using Serilog;

namespace ResumeGenerator.ApiService.Web;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenApi();

        services.AddEndpointsApiExplorer();
        services.AddSwagger();

        services.AddDataLayerServices(_configuration);
        
        services.AddControllers();

        services.AddValidators();
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddApplicationServices();
        services.AddHandlers();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        services.AddSingleton<ErrorHandlingMiddleware>();
        services.AddAsyncInitializer<MigrationAsyncInitializer>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        if (_environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(e => e.MapControllers());
    }
}