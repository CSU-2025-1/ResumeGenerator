using HtmlToPdfMaster;
using Microsoft.Extensions.Options;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Infrastructure.Generating;

public sealed class ResumeGenerator : IResumeGenerator
{
    private readonly string _template;

    public ResumeGenerator(IOptions<ResumeTemplates> options)
    {
        string templatePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, options.Value.TemplateFolderPath, options.Value.PdfTemplateName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"HTML template file not found on path:\n{templatePath}");
        }

        _template = File.ReadAllText(templatePath);
    }

    public byte[] GeneratePdf(in Resume resume, in PdfParameters parameters) => HtmlConverter.FromHtmlString(
        GenerateHtml(resume),
        parameters.Width,
        parameters.Height,
        parameters.Quality,
        parameters.Dpi,
        parameters.MarginLeft,
        parameters.MarginTop,
        parameters.MarginRight,
        parameters.MarginBottom,
        parameters.Encoding
    );

    public string GenerateHtml(in Resume resume) => string.Format(_template, [
        resume.FirstName, resume.MiddleName, resume.LastName,
        resume.DesiredPosition,
        resume.GitHubLink,
        resume.TelegramLink,
        resume.Email,
        resume.PhoneNumber,
        resume.Education,
        resume.Experience,
        string.Join(", ", resume.HardSkills),
        string.Join(", ", resume.SoftSkills)
    ]);
}