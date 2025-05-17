using FluentValidation;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

namespace ResumeGenerator.ApiService.Application.Validators.Resumes;

public sealed class GetResumeByIdRequestValidator : AbstractValidator<GetResumeByIdRequest>
{
    public GetResumeByIdRequestValidator()
    {
        RuleFor(request => request.ResumeId)
            .NotEqual(Guid.Empty)
            .NotEmpty();
        
        RuleFor(request => request.UserId)
            .NotEqual(Guid.Empty)
            .NotEmpty();
    }
}