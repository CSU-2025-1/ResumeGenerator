using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.AuthService.Application.Configuration;
using ResumeGenerator.AuthService.Application.Services;
using ResumeGenerator.AuthService.Data.Context;
using ResumeGenerator.AuthService.Grpc;
using ResumeGenerator.AuthService.Web.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(TelegramBotOptions.SectionName));

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IBotLinkGenerator, TelegramBotLinkGenerator>();
builder.Services.AddScoped<ResumeGenerator.AuthService.Grpc.AuthInterceptor>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

await app.InitAndRunAsync();
