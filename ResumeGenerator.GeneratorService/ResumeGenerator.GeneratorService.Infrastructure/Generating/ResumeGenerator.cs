using HtmlToPdf2AZ;
using HtmlToPdf2AZ.Models;
using Microsoft.Extensions.Options;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Infrastructure.Generating;

public sealed class ResumeGenerator : IResumeGenerator
{
    private static readonly PdfTools PdfTools = new();

    private readonly string _template;
    private readonly string _style;

    public ResumeGenerator(IOptions<ResumeTemplates> options)
    {
        string folder = options.Value.TemplateFolderPath;
        _template = LoadFile(folder, options.Value.PdfTemplateName, "HTML template");
        _style = LoadFile(folder, options.Value.PdfStyleName, "CSS style");
    }

    public Task<Stream> GeneratePdf(
        in Resume resume, MarginOptions marginOptions, PaperFormat paperFormat) => PdfTools.GetPDFFromHTML(
            htmlContent: GenerateHtml(resume),
            marginOptions: marginOptions,
            paperFormat: paperFormat);

    public string GenerateHtml(in Resume resume) => string.Format(_template, _style,
        resume.FirstName, resume.MiddleName, resume.LastName,
        resume.DesiredPosition,
        resume.GitHubLink,
        resume.TelegramLink,
        resume.Email,
        resume.PhoneNumber,
        resume.Education,
        resume.ExperienceYears.ToString(),
        string.Join(", ", resume.HardSkills),
        string.Join(", ", resume.SoftSkills)
    );

    private static string LoadFile(string folder, string name, string fileTypeName)
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder, name);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"{fileTypeName} file not found on path:\n{path}");
        }

        return File.ReadAllText(path);
    }
}
