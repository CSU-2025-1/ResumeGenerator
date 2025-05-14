using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ResumeGenerator.TelegramAdapter.Core;
using ResumeGenerator.TelegramAdapter.Core.Extensions;
using ResumeGenerator.TelegramAdapter.Grpc.Clients.Generated;
using ResumeGenerator.TelegramAdapter.Grpc.Server.Services;
using ResumeGenerator.TelegramAdapter.Infrastructure.Minio.Extensions;
using ResumeGenerator.TelegramAdapter.Infrastructure.Persistence;
using ResumeGenerator.TelegramAdapter.Infrastructure.Persistence.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

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

string? conString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTelegramBotClient(telegramBotClient);
builder.Services.AddPersistence(conString);

builder.Services.AddMinio(builder.Configuration.GetSection("Minio"));

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<AuthServiceGrpc.AuthServiceGrpcClient>();

var app = builder.Build();

app.MapGrpcService<TelegramAdapterService>();
app.MapPost("/telegram-adapter/v1/updates", (
    [FromBody] Update update,
    [FromServices] UpdateDispatcher updateDispatcher,
    CancellationToken ct) => updateDispatcher.DispatchAsync(update, ct));

var migrator = new Migrator(conString);
migrator.Migrate();

app.Run();