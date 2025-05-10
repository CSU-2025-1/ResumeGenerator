namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(string username)
        : base($"User with username '{username}' not found") { }
}