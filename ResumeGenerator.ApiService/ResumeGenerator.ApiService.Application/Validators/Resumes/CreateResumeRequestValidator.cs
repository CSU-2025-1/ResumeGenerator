using FluentValidation;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Dto;

namespace ResumeGenerator.ApiService.Application.Validators.Resumes;

public sealed class CreateResumeRequestValidator : AbstractValidator<CreateResumeRequest>
{
    public CreateResumeRequestValidator(ResumeDtoValidator resumeDtoValidator)
    {
        RuleFor(request => request.Resume)
            .SetValidator(resumeDtoValidator);
    }
}