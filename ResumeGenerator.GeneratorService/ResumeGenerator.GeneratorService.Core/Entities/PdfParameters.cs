namespace ResumeGenerator.GeneratorService.Core.Entities;

public readonly record struct PdfParameters(
    int Width, 
    int Height, 
    int Quality = 100, 
    int Dpi = 100, 
    int MarginLeft = 0, 
    int MarginTop = 0, 
    int MarginRight = 0, 
    int MarginBottom = 0, 
    string Encoding = "UTF-8"
);
