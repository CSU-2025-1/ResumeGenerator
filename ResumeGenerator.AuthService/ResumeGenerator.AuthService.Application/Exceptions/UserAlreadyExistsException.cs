namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
        : base($"User account is already exists") { }
}