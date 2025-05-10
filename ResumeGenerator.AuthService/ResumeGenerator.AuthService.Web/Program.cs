using ResumeGenerator.AuthService.Application.Configuration;
using ResumeGenerator.AuthService.Web.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(TelegramBotOptions.SectionName));

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IBotLinkGenerator, TelegramBotLinkGenerator>();

// Add validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply migrations
await app.MigrateDatabaseAsync();

app.Run();