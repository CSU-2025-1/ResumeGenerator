using System.Text.RegularExpressions;
using FluentValidation;
using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.Validation;

namespace ResumeGenerator.AuthService.Application.Validators;

public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Username).ApplyUsernameRules();
        RuleFor(x => x.Password).ApplyPasswordRules();
    }
}