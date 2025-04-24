using MassTransit;
using ResumeGenerator.Common.Contracts;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Worker;

public sealed class CreateResumeCommandConsumer : IConsumer<CreateResumeCommand>
{
    private static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "MyPdf.pdf");
    private readonly IResumeGenerator _resumeGenerator;

    public CreateResumeCommandConsumer(IResumeGenerator resumeGenerator)
    {
        _resumeGenerator = resumeGenerator;
    }

    public async Task Consume(ConsumeContext<CreateResumeCommand> context)
    {
        var parameters = new PdfParameters(148, 210);
        var command = context.Message;
        
        await File.WriteAllBytesAsync(Path, _resumeGenerator.GeneratePdf(new Resume
        {
            ResumeId = command.ResumeId,
            UserId = command.UserId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            MiddleName = command.MiddleName,
            DesiredPosition = command.DesiredPosition,
            GitHubLink = command.GitHubLink,
            TelegramLink = command.TelegramLink,
            Email =  command.Email,
            PhoneNumber = command.PhoneNumber,
            Education = command.Education,
            ExperienceYears = command.ExperienceYears,
            HardSkills = command.HardSkills,
            SoftSkills = command.SoftSkills,
        }, parameters), context.CancellationToken);
    }
}
