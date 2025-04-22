using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Worker;

public static class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.Configure<ResumeTemplates>(
            builder.Configuration.GetSection(nameof(ResumeTemplates))
        );

        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<IResumeGenerator, Infrastructure.Generating.ResumeGenerator>();

        IHost host = builder.Build();
        host.Run();
    }
}
