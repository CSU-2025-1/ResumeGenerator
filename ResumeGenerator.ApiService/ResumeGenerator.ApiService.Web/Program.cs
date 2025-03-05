using ResumeGenerator.ApiService.Application.Extensions;
using ResumeGenerator.ApiService.Application.Mapping;
using ResumeGenerator.ApiService.Data.Extentions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDataLayerServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddValidators();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddApplicationServices();
builder.Services.AddHandlers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    
}