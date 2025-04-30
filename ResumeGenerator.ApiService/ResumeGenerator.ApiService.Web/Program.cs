namespace ResumeGenerator.ApiService.Web;

internal static class Program
{
    public static async Task Main()
    {
        await Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                    .UseKestrel(options =>
                    {
                        options.ListenLocalhost(8080, listenOptions =>
                        {
                            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                        });
                    })
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.Development.json")
                        .Build());
            })
            .Build()
            .InitAndRunAsync();
    }
}