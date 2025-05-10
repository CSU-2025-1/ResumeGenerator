namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string username)
        : base($"User with username '{username}' already exists") { }
}