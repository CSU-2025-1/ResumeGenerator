using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ResumeGenerator.GeneratorService.Infrastructure.Templates;

public sealed class SectionComponent : IComponent
{
    private readonly string _title;
    private readonly string _content;

    public SectionComponent(string title, string content)
    {
        _title = title;
        _content = content;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Item()
                .PaddingTop(20)
                .Text(_title)
                .FontColor(Colors.Blue.Darken2)
                .FontFamily(Fonts.Arial)
                .FontSize(18);

            column.Item()
                .PaddingTop(20)
                .Text(_content)
                .FontFamily(Fonts.Arial);
        });
    }
}