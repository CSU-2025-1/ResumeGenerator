using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Worker;

public class Worker(ILogger<Worker> logger, IResumeGenerator resumeGenerator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        /*
        string path = Path.Combine(Directory.GetCurrentDirectory(), "MyPdf.pdf");
        
        var resume = new Resume("Михаил", "Бекасов", "Юрьевич", "Разраб",
            "https://github.com/MichaelGallinago", "https://t.me/micsnipe", "Не дам",
            "88005553535", "Сами с усами", "Немеренно",
            ["C#", "Git"], ["Злой", "Негативный"]);
        
        var parameters = new PdfParameters(148, 210);
        
        await File.WriteAllBytesAsync(path, resumeGenerator.GeneratePdf(resume, parameters), stoppingToken);
        */
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
