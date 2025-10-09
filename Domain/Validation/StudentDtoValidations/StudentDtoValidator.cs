using Domain.DTOs.StudentDTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Validation.StudentDtoValidations;

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    public StudentDtoValidator()
    {
        RuleFor(x => x.Id)
              .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage("First name is required.")
              .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(_ => DateTime.Today.AddYears(-5)).WithMessage("Student must be at least 5 years old.")
            .GreaterThanOrEqualTo(_ => DateTime.Today.AddYears(-120)).WithMessage("Date of birth is unrealistically old.")
            .LessThan(_ => DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.IdNumber)
            .NotEmpty().WithMessage("ID number is required.")
            .Length(6, 20)
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("ID number can contain letters, numbers, and dashes only.");

        When(x => x.Picture is not null, () =>
        {
            RuleFor(x => x.Picture!)
                .NotEmpty().WithMessage("Picture cannot be empty when provided.")
                .Must(IsValidImageReference)
                .WithMessage("Picture must be a valid absolute URL or a data URI (base64).");
        });
    }
    private static bool IsValidImageReference(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        // Accept absolute URLs
        if (Uri.IsWellFormedUriString(value, UriKind.Absolute)) return true;

        // Accept simple data URI forms: data:image/<type>;base64,<payload>
        if (value.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
        {
            var commaIndex = value.IndexOf(',');
            if (commaIndex <= 0) return false;

            var header = value[..commaIndex];
            if (!header.Contains(";base64", StringComparison.OrdinalIgnoreCase)) return false;

            var base64 = value[(commaIndex + 1)..];
            // Quick base64 sanity check
            return Regex.IsMatch(base64, @"^[A-Za-z0-9+/=\r\n]+$");
        }

        return false;
    }
}