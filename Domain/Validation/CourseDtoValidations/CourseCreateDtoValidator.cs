using Domain.DTOs.CourseDTOs;
using FluentValidation;

namespace Domain.Validation.CourseDtoValidations;

public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 10);
    }
}