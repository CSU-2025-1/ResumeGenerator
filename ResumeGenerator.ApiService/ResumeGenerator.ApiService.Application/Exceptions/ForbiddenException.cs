using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public sealed class ForbiddenException : ExceptionBase
{
    public override int StatusCode { get; } = StatusCodes.Status403Forbidden;

    public static void ThrowWithError(Error error)
        => throw new ForbiddenException
        {
            Error = error
        };
}