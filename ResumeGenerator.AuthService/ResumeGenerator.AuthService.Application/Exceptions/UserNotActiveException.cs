namespace ResumeGenerator.AuthService.Application.Exceptions;

public sealed class UserNotActiveException : Exception
{
    public UserNotActiveException()
        : base("User account is not activated") { }
}