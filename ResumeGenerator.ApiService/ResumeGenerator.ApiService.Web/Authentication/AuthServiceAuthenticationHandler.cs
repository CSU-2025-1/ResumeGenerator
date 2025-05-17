using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using ResumeGenerator.ApiService.Application.Services.Auth;

namespace ResumeGenerator.ApiService.Web.Authentication;

public sealed class AuthServiceAuthenticationHandler : AuthenticationHandler<AuthServiceAuthenticationOptions>
{
    private readonly IAuthService _authService;

    [Obsolete("Obsolete")]
    public AuthServiceAuthenticationHandler(
        IOptionsMonitor<AuthServiceAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IAuthService authService) : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    public AuthServiceAuthenticationHandler(
        IOptionsMonitor<AuthServiceAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IAuthService authService) : base(options, logger, encoder)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var value))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string token = value.ToString().Replace("Bearer ", string.Empty);
            var userId = await _authService.GetUserIdFromTokenAsync(token, Context.RequestAborted);

            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString())
            ];

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(claimsIdentity);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}