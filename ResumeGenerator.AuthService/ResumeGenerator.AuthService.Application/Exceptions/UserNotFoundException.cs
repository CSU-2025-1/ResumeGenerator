namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException()
        : base($"User account is not found") { }
}