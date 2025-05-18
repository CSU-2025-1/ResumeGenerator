using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.AuthService.Application.Configuration;
using ResumeGenerator.AuthService.Application.Services;
using ResumeGenerator.AuthService.Data.Context;
using ResumeGenerator.AuthService.Grpc;
using ResumeGenerator.AuthService.Web.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenLocalhost(8080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
    options.ListenLocalhost(8081, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(TelegramBotOptions.SectionName));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(IAuthService).Assembly); //регистрирует все валидаторы из application
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IBotLinkGenerator, TelegramBotLinkGenerator>();
builder.Services.AddScoped<ResumeGenerator.AuthService.Grpc.AuthGrpcService>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<AuthInterceptor>();
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAsyncInitializer<DatabaseInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<AuthGrpcService>();

await app.InitAndRunAsync();
