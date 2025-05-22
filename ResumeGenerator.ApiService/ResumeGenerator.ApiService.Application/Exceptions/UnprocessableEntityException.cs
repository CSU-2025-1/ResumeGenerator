using Microsoft.AspNetCore.Http;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public class UnprocessableEntityException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status422UnprocessableEntity;
}