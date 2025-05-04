namespace ResumeGenerator.AuthService.Application.Exceptions;

public class UserNotActiveException : Exception
{
    public UserNotActiveException()
        : base("User account is not activated") { }
}