using FluentValidation;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

namespace ResumeGenerator.ApiService.Application.Validators.Resumes;

public class GetResumesByUserIdRequestValidator: AbstractValidator<GetResumesByUserIdRequest>
{
    public GetResumesByUserIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEqual(Guid.Empty);
    }
}