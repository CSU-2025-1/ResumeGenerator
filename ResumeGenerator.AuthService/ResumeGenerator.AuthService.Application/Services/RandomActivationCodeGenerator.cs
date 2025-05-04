namespace ResumeGenerator.AuthService.Application.Services;

public class RandomActivationCodeGenerator : IActivationCodeGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Length = 16;

    public string GenerateCode()
    {
        var random = new Random();
        return new string(Enumerable.Repeat(Chars, Length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
}