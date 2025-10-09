using Domain.DTOs.AuthenticationDtos;
using FluentValidation;

namespace Domain.Validation.RegisterDtoValidations;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
        .EmailAddress();

        RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);
    }
}
