using FluentValidation.Results;
namespace ResumeGenerator.ApiService.Application.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error FromValidationFailure(ValidationFailure failure)
        => new(failure.ErrorCode, failure.ErrorMessage);
}