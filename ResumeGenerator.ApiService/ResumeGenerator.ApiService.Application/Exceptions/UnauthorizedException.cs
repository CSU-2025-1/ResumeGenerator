using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public class UnauthorizedException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;

    public static void ThrowByError(Error error)
    {
        throw new UnauthorizedException
        {
            Error = error
        };
    }
}