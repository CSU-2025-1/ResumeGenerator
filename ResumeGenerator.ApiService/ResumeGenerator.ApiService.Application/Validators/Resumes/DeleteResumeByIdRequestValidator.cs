using FluentValidation;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

namespace ResumeGenerator.ApiService.Application.Validators.Resumes;

public sealed class DeleteResumeByIdRequestValidator : AbstractValidator<DeleteResumeByIdRequest>
{
    public DeleteResumeByIdRequestValidator()
    {
        RuleFor(request => request.ResumeId)
            .NotEqual(Guid.Empty)
            .NotEmpty();
        
        RuleFor(request => request.UserId)
            .NotEqual(Guid.Empty)
            .NotEmpty();
    }
}