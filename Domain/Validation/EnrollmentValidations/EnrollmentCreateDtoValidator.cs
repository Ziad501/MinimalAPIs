using Domain.DTOs.EnrollmentDTOs;
using FluentValidation;

namespace Domain.Validation.EnrollmentValidations;

public class EnrollmentCreateDtoValidator : AbstractValidator<EnrollmentCreateDto>
{
    public EnrollmentCreateDtoValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.StudentId).GreaterThan(0);
    }
}