using Domain.DTOs.CourseDTOs;
using FluentValidation;

namespace Domain.Validation.CourseDtoValidations;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 10);
    }
}