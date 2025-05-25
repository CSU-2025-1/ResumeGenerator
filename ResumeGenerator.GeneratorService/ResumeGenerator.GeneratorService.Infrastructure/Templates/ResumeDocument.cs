using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ResumeGenerator.GeneratorService.Core.Entities;

namespace ResumeGenerator.GeneratorService.Infrastructure.Templates;

public sealed class ResumeDocument : IDocument
{
    private readonly Resume _resume;

    public ResumeDocument(Resume resume)
    {
        _resume = resume;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(40);

            page.Content().Border(2).BorderColor(Colors.Black).Padding(20).Column(column =>
            {
                // Horizontal line
                column.Item()
                    .BorderBottom(1)
                    .BorderColor(Colors.Black)
                    .PaddingTop(20)
                    .PaddingBottom(20);

                // Header section
                column.Item().AlignCenter().Column(header =>
                {
                    header.Item()
                        .PaddingBottom(5)
                        .Text($"{_resume.LastName} {_resume.FirstName} {_resume.MiddleName}")
                        .FontFamily(Fonts.Arial)
                        .FontSize(28).Bold();

                    header.Item()
                        .Text(_resume.DesiredPosition)
                        .FontSize(20)
                        .FontFamily(Fonts.Arial)
                        .FontColor(Colors.Green.Darken1);
                });

                // Contacts block
                column.Item().PaddingVertical(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().Text($"Телефон: {_resume.PhoneNumber}").FontFamily(Fonts.Arial);
                    table.Cell().Text($"Email: {_resume.Email}").FontFamily(Fonts.Arial);
                });

                // Skills sections
                column.Item().Component(new SectionComponent("Хард Скилы", string.Join(", ", _resume.HardSkills)));
                column.Item().Component(new SectionComponent("Софт Скилы", string.Join(", ", _resume.SoftSkills)));
                column.Item().Component(new SectionComponent("Образование", _resume.Education));
                column.Item().Component(new SectionComponent("Опыт работы (лет)", _resume.ExperienceYears.ToString()));

                // Links
                column.Item().PaddingVertical(10).Column(links =>
                {
                    links.Item()
                        .Text("GitHub:")
                        .FontColor(Colors.Blue.Darken2)
                        .FontSize(18)
                        .FontFamily(Fonts.Arial);
                    links.Item()
                        .Hyperlink(_resume.GitHubLink)
                        .Text(_resume.GitHubLink)
                        .FontFamily(Fonts.Arial);

                    links.Item()
                        .PaddingTop(10)
                        .Text("Telegram:")
                        .FontColor(Colors.Blue.Darken2)
                        .FontSize(18)
                        .FontFamily(Fonts.Arial);
                    links.Item()
                        .Hyperlink(_resume.TelegramLink)
                        .Text(_resume.TelegramLink)
                        .FontFamily(Fonts.Arial);
                });
            });
        });
    }
}