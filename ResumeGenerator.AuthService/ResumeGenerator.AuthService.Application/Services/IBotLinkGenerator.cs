namespace ResumeGenerator.AuthService.Application.Services;

public interface IBotLinkGenerator
{
    string GenerateLink(string guidUserId);
}