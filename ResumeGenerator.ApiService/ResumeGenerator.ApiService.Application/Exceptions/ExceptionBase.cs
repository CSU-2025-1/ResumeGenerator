
using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Application.Exceptions;

public abstract class ExceptionBase : Exception
{
    public abstract int StatusCode { get; }
    public Error Error { get; init; } = Error.None;
}