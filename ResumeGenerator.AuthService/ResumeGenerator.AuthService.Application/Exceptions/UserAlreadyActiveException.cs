namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserAlreadyActiveException : Exception
{
    public UserAlreadyActiveException()
        : base($"User account is already activated.") { }
}