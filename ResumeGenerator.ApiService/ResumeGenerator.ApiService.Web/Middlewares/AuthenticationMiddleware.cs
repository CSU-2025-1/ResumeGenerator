using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.AuthClientGrpc;

namespace ResumeGenerator.ApiService.Web.Middlewares;

public sealed class AuthenticationMiddleware : IMiddleware
{
    private readonly IAuthService _authService;

    public AuthenticationMiddleware(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {      
        var path = context.Request.Path.Value;
        if (path.StartsWith("/swagger") || path.StartsWith("/favicon.ico"))
        {
            await next(context);
            return;
        }
        
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            UnauthorizedException.ThrowWithError(new Error(StatusCodes.Status401Unauthorized.ToString(),
                $"Access token is missing or invalid."));
        }

        try
        {
            var userId = await _authService.GetUserIdFromTokenAsync(token, context.RequestAborted);
            context.Items["UserId"] = userId;
        }
        catch (UnauthorizedAccessException)
        {
            UnauthorizedException.ThrowWithError(new Error(StatusCodes.Status401Unauthorized.ToString(),
                $"Access token is expired."));
        }

        await next(context);
    }
}