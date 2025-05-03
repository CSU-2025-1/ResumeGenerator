using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.Entities;
using ResumeGenerator.TelegramAdapter.Grpc.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8080, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });

    options.ListenLocalhost(8081, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});

builder.Services.AddGrpc();
builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);

var telegramBotClient = new TelegramBotClient(builder.Configuration["Telegram:BotToken"] ?? string.Empty);
await telegramBotClient.SetWebhook(
    url: builder.Configuration["Telegram:Webhook"] ?? string.Empty,
    allowedUpdates: [UpdateType.Message]);

builder.Services.Add(
    ServiceDescriptor.Describe(
        serviceType: typeof(ITelegramBotClient),
        implementationType: typeof(TelegramBotClient),
        lifetime: ServiceLifetime.Singleton)
);

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
        UserId = Guid.Empty
    }, ct);
    if (result.IsFailure)
    {
        Console.WriteLine(result.Error);
    }
});

app.Run();