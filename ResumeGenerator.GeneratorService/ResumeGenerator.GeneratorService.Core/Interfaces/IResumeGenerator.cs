using HtmlToPdf2AZ.Models;
using ResumeGenerator.GeneratorService.Core.Entities;

namespace ResumeGenerator.GeneratorService.Core.Interfaces;

public interface IResumeGenerator
{
    Task<Stream> GeneratePdfAsync(
        in Resume resume, MarginOptions marginOptions, PaperFormat paperFormat, CancellationToken ct = default);

    string GenerateHtml(in Resume resume);
}
