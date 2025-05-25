using FluentValidation;
using System.Text.RegularExpressions;

public static class UsernameValidationRules
{
    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100)
            .Must(ContainDigit).WithMessage("Пароль должен содержать хотя бы одну цифру (0-9)")
            .Must(ContainUppercase).WithMessage("Пароль должен содержать хотя бы одну заглавную букву (A-Z)")
            .Must(ContainLowercase).WithMessage("Пароль должен содержать хотя бы одну строчную букву (a-z)");
    }

    private static bool ContainDigit(string password) => Regex.IsMatch(password, @"[0-9]");
    private static bool ContainUppercase(string password) => Regex.IsMatch(password, @"[A-Z]");
    private static bool ContainLowercase(string password) => Regex.IsMatch(password, @"[a-z]");
}
