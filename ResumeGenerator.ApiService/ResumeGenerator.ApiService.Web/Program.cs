namespace ResumeGenerator.ApiService.Web;

internal static class Program
{
    public static async Task Main()
    {
        await Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.Development.json")
                        .Build());
            })
            .Build()
            .InitAndRunAsync();
    }
}