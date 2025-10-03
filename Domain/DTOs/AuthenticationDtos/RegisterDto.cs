namespace Domain.DTOs.AuthenticationDtos;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateTime DateOfBirth
    );