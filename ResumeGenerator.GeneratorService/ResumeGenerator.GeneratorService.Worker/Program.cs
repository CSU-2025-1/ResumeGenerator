using MassTransit;
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

        //builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<IResumeGenerator, Infrastructure.Generating.ResumeGenerator>();
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateResumeCommandConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
                cfg.ReceiveEndpoint("created-resumes", y =>
                {
                    y.ConfigureConsumeTopology = true;

                    y.Consumer<CreateResumeCommandConsumer>(context);
                });
            });
        });

        IHost host = builder.Build();
        host.Run();
    }
}
