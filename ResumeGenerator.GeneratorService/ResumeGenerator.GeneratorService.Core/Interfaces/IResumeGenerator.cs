using ResumeGenerator.GeneratorService.Core.Entities;

namespace ResumeGenerator.GeneratorService.Core.Interfaces;

public interface IResumeGenerator
{
    Stream GeneratePdf(in Resume resume);

    string GenerateHtml(in Resume resume);
}
