using MassTransit;
using Minio;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;
using ResumeGenerator.GeneratorService.Grpc.Generated;

namespace ResumeGenerator.GeneratorService.Worker;

public static class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.Configure<ResumeTemplates>(
            builder.Configuration.GetSection(nameof(ResumeTemplates))
        );

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddHostedService<Worker>();
        }

        builder.Services.AddSingleton<IResumeGenerator, Infrastructure.Generating.ResumeGenerator>();
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateResumeCommandConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration["RabbitMq:Host"], builder.Configuration["RabbitMq:VirtualHost"], h =>
                {
                    h.Username(builder.Configuration["RabbitMq:Username"]);
                    h.Password(builder.Configuration["RabbitMq:Password"]);
                });

                cfg.ConfigureEndpoints(context);
                cfg.ReceiveEndpoint("created-resumes", y =>
                {
                    y.ConfigureConsumeTopology = true;

                    y.Consumer<CreateResumeCommandConsumer>(context);
                });
            });
        });

        ConfigurationManager configuration = builder.Configuration;

        builder.Services.AddMinio(configureClient => configureClient
            .WithEndpoint(configuration["MINIO_ENDPOINT"])
            .WithCredentials(configuration["MINIO_ACCESS_KEY"], configuration["MINIO_SECRET_KEY"])
            .WithSSL(false) // true for https
            .Build());

        builder.Services.AddGrpcClient<TelegramAdapter.TelegramAdapterClient>(o =>
        {
            o.Address = new Uri(configuration["TELEGRAM_GRPC_ENDPOINT"] ?? string.Empty);
        });

        builder.Services.AddGrpcClient<ResumeServiceGrpc.ResumeServiceGrpcClient>(o =>
        {
            o.Address = new Uri(configuration["API_GRPC_ENDPOINT"] ?? string.Empty);
        });

        IHost host = builder.Build();
        host.Run();
    }
}
