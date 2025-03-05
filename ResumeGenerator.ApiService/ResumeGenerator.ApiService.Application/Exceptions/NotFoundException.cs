using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public class NotFoundException : ExceptionBase
{
    public override int StatusCode { get; } = StatusCodes.Status404NotFound;

    public static void ThrowIfNull(object? o, Error err)
    {
        if (o is null)
        {
            throw new NotFoundException
            {
                Error = err
            };
        }
    }
}