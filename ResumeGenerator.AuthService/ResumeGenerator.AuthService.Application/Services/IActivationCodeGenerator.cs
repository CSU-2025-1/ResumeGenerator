namespace ResumeGenerator.AuthService.Application.Services;

public interface IActivationCodeGenerator
{
    string GenerateCode();
}