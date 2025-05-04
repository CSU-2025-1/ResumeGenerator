using FluentValidation;
using ResumeGenerator.AuthService.Application.DTO.Requests;

namespace ResumeGenerator.AuthService.Application.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}