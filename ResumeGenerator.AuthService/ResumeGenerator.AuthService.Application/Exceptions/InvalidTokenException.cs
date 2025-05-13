namespace ResumeGenerator.AuthService.Application.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(Exception innerException)
        : base("Invalid JWT token ", innerException) { }
}