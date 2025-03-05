using HtmlToPdfMaster;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Infrastructure.Generating;

public class ResumeGenerator : IResumeGenerator
{
    private static readonly string TemplatePath = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ResumeTemplate.html");
    
    private static readonly string Template;

    static ResumeGenerator()
    {
        if (!File.Exists(TemplatePath))
            throw new FileNotFoundException($"HTML template file not found on path:\n{TemplatePath}");
        
        Template = File.ReadAllText(TemplatePath);
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

    private static string GenerateHtml(in Resume resume) => string.Format(Template, [
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
