using Domain.DTOs.EnrollmentDTOs;
using FluentValidation;

namespace Domain.Validation.EnrollmentValidations;

public class EnrollmentDtoValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.StudentId).GreaterThan(0);
    }
}