using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Minio;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.Entities;
using ResumeGenerator.TelegramAdapter.Grpc.Services;
using ResumeGenerator.TelegramAdapter.Infrastructure.Minio;
using ResumeGenerator.TelegramAdapter.Infrastructure.Persistence;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using MinioConfig = ResumeGenerator.TelegramAdapter.Infrastructure.Minio.MinioConfig;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8080, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });

    options.ListenLocalhost(8081, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});

builder.Services.AddGrpc();
builder.Services.ConfigureTelegramBot<JsonOptions>(opt => opt.SerializerOptions);

var telegramBotClient = new TelegramBotClient(builder.Configuration["Telegram:BotToken"] ?? string.Empty);
await telegramBotClient.SetWebhook(
    url: builder.Configuration["Telegram:Webhook"] ?? string.Empty,
    allowedUpdates: [UpdateType.Message]);

builder.Services.Add(
    ServiceDescriptor.Describe(
        serviceType: typeof(ITelegramBotClient),
        implementationFactory: _ => telegramBotClient,
        lifetime: ServiceLifetime.Singleton)
);

builder.Services.Add(
    ServiceDescriptor.Describe(
        serviceType: typeof(DbConnectionFactory),
        implementationFactory: _ => new DbConnectionFactory(
            builder.Configuration.GetConnectionString("DefaultConnection")),
        lifetime: ServiceLifetime.Singleton)
);
builder.Services.AddScoped<ITelegramChatRepository, TelegramChatRepository>();

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(builder.Configuration["Minio:Endpoint"])
    .WithCredentials(builder.Configuration["Minio:AccessKey"],  builder.Configuration["Minio:SecretKey"])
    .WithSSL(false)
    .Build());
builder.Services.Configure<MinioConfig>(builder.Configuration.GetSection("Minio"));
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();

var app = builder.Build();

app.MapGrpcService<TelegramAdapterService>();
app.MapPost("/telegram-adapter/v1/updates", async (
    [FromBody] Update update,
    [FromServices] ITelegramChatRepository telegramChatRepository,
    CancellationToken ct) =>
{
    var result = await telegramChatRepository.SaveChatAsync(new TelegramChat
    {
        ExtId = update.Message!.Chat.Id,
        UserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
    }, ct);
    if (result.IsFailure)
    {
        Console.WriteLine(result.Error);
    }
});

var migrator = new Migrator(builder.Configuration.GetConnectionString("DefaultConnection"));
migrator.Migrate();

app.Run();