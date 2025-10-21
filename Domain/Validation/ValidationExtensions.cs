using Domain.DTOs.AuthenticationDtos;
using FluentValidation;

namespace Domain.Validation;

public static class ValidationExtensions
{
    public static async Task<List<ErrorResponseDto>?> ToErrorsAsync<T>(
        this IValidator<T> validator, T dto, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(dto, ct);
        if (result.IsValid) return null;

        return result.Errors.Select(e =>
            new ErrorResponseDto(
                string.IsNullOrEmpty(e.ErrorCode) ? e.PropertyName : e.ErrorCode,
                e.ErrorMessage)).ToList();
    }
}