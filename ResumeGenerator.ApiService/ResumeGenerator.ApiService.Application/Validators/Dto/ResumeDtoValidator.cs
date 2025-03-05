using FluentValidation;
using ResumeGenerator.ApiService.Application.DTO;

namespace ResumeGenerator.ApiService.Application.Validators.Dto;

public class ResumeDtoValidator: AbstractValidator<ResumeDto>
{
    public ResumeDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .Null();

        RuleFor(dto => dto.UserFirstName)
            .NotEmpty()
            .Length(1, 50);

        RuleFor(dto => dto.UserLastName)
            .NotEmpty()
            .Length(1, 50);

        RuleFor(dto => dto.UserPatronymic)
            .NotEmpty()
            .Length(1, 50);

        RuleFor(dto => dto.DesiredPosition)
            .NotEmpty()
            .Length(1, int.MaxValue);

        RuleFor(dto => dto.GitHubLink)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrNonEmptyValidator")
            .WithMessage("GitHubLink must be null or non-empty");

        RuleFor(dto => dto.TelegramLink)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrNonEmptyValidator")
            .WithMessage("TelegramLink must be null or non-empty");

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(dto => dto.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(dto => dto.Education)
            .NotEmpty()
            .Length(1, int.MaxValue);

        RuleFor(dto => dto.ExperienceYears)
            .GreaterThanOrEqualTo(0);

        RuleFor(dto => dto.HardSkills)
            .NotEmpty()
            .Length(1, int.MaxValue);

        RuleFor(dto => dto.SoftSkills)
            .NotEmpty()
            .Length(1, int.MaxValue);
    }
}