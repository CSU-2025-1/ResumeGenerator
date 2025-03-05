using ResumeGenerator.GeneratorService.Core.Entities;

namespace ResumeGenerator.GeneratorService.Core.Interfaces;

public interface IResumeGenerator
{
    byte[] GeneratePdf(in Resume resume, in PdfParameters parameters);
    string GenerateHtml(in Resume resume);
}
