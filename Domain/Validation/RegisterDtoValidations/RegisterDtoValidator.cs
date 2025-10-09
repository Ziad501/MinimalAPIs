using Domain.DTOs.AuthenticationDtos;
using FluentValidation;

namespace Domain.Validation.RegisterDtoValidations;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(BeInThePast).WithMessage("'DateOfBirth' must be in the past.")
            .Must(d => BeAtLeastYearsOld(d, 16)).WithMessage("User must be at least 16 years old.");
    }

    private static bool BeInThePast(DateTime d) => d.Date < DateTime.UtcNow.Date;
    private static bool BeAtLeastYearsOld(DateTime dob, int years)
        => dob <= DateTime.UtcNow.Date.AddYears(-years);
}
