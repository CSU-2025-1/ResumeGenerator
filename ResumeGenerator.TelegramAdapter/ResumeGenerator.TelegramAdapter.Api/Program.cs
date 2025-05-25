using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ResumeGenerator.TelegramAdapter.Core;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;
using ResumeGenerator.TelegramAdapter.Core.UpdateHandlers;
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
    options.ListenAnyIP(8080, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });

    options.ListenAnyIP(8081, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});

builder.Services.AddGrpc();
builder.Services.ConfigureTelegramBot<JsonOptions>(opt => opt.SerializerOptions);
builder.Services.AddScoped<UpdateDispatcher>();
builder.Services.AddScoped<IUpdateHandler, StartCommandUpdateHandler>();

var telegramBotClient = new TelegramBotClient(builder.Configuration["Telegram:BotToken"] ?? string.Empty);
await telegramBotClient.SetWebhook(
    url: builder.Configuration["Telegram:Webhook"] ?? string.Empty,
    allowedUpdates: [UpdateType.Message]);

string? conString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTelegramBotClient(telegramBotClient);
builder.Services.AddPersistence(conString);

builder.Services.AddMinio(builder.Configuration.GetSection("Minio"));

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<AuthServiceGrpc.AuthServiceGrpcClient>(
    o => o.Address = new Uri(builder.Configuration["AuthService:Url"])
);

var app = builder.Build();

app.MapGrpcService<TelegramAdapterService>();
app.MapPost("/telegram-adapter/v1/updates", async (
    [FromBody] Update update,
    [FromServices] UpdateDispatcher updateDispatcher,
    CancellationToken ct) =>
{
    await updateDispatcher.DispatchAsync(update, ct);
    Console.WriteLine("Done");
    return Results.Ok();
});

var migrator = new Migrator(conString);
migrator.Migrate();

app.Run();