using Microsoft.AspNetCore.Authentication;

namespace ResumeGenerator.ApiService.Web.Authentication;

public sealed class AuthServiceAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "AuthService";
}