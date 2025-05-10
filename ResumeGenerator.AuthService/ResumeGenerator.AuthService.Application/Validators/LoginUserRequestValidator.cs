using System.Text.RegularExpressions;
using FluentValidation;
using ResumeGenerator.AuthService.Application.DTO.Requests; 

namespace ResumeGenerator.AuthService.Application.Validators;

public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Пароль обязателен")
            .MinimumLength(8)
            .WithMessage("Пароль должен содержать минимум 8 символов")
            .Must(ContainDigit)
            .WithMessage("Пароль должен содержать хотя бы одну цифру (0-9)")
            .Must(ContainUppercase)
            .WithMessage("Пароль должен содержать хотя бы одну заглавную букву (A-Z)")
            .Must(ContainLowercase)
            .WithMessage("Пароль должен содержать хотя бы одну строчную букву (a-z)");
    }

    private bool ContainDigit(string password)
        => Regex.IsMatch(password, @"[0-9]");

    private bool ContainUppercase(string password)
        => Regex.IsMatch(password, @"[A-Z]");

    private bool ContainLowercase(string password)
        => Regex.IsMatch(password, @"[a-z]");
}