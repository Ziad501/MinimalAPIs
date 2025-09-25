namespace Domain.DTOs.StudentDTOs;

public record StudentCreateDto(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string IdNumber,
    string Picture
    );
