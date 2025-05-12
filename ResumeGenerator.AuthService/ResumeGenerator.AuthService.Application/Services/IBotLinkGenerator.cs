namespace ResumeGenerator.AuthService.Application.Services;

public interface IBotLinkGenerator
{
    string GenerateLink(Guid userId);
}