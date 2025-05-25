using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IResumeGenerator _resumeGenerator;
    
    public Worker(ILogger<Worker> logger, IResumeGenerator resumeGenerator)
    {
        _logger = logger;
        _resumeGenerator = resumeGenerator;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "MyPdf.pdf");
        
        var resume = new Resume(Guid.Empty, Guid.Empty,
            "Михаил", "Бекасов", "Юрьевич", "Разраб",
            "https://github.com/MichaelGallinago", "https://t.me/micsnipe", "Не дам",
            "88005553535", "Сами с усами", 6,
            ["C#", "Git"], ["Злой", "Негативный"]);

        await using FileStream output = File.Create(path);

        _logger.LogInformation(path);
    }
}
