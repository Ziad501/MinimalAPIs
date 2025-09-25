namespace Domain.DTOs.StudentDTOs;

public record StudentDto(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string IdNumber,
    string Picture
    );