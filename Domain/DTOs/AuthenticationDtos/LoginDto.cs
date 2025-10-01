namespace Domain.DTOs.AuthenticationDtos;

public record LoginDto(
    string Email,
    string Password
);