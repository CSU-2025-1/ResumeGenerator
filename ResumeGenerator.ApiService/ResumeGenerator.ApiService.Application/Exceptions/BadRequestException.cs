using ResumeGenerator.ApiService.Application.Results;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public sealed class BadRequestException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status400BadRequest;

    public static void ThrowByValidationResult(ValidationResult result)
    {
        if (!result.IsValid)
        {
            throw new BadRequestException
            {
                Error = Error.FromValidationFailure(result.Errors[0])
            };
        }
    }
}