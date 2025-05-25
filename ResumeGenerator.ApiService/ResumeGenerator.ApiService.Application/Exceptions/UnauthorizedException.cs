using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public sealed class UnauthorizedException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;

    public static void ThrowWithError(Error error)
        => throw new UnauthorizedException
        {
            Error = error
        };
}