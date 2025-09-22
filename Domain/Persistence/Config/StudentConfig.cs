using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Persistence.Config;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        var seedDate = new DateTime(2025, 9, 20, 16, 53, 0, DateTimeKind.Utc);
        const string seedUser = "System";
        builder.HasData(
            new Student { Id = 1, FirstName = "Ali", LastName = "Hassan", DateOfBirth = new DateTime(2005, 1, 15), IdNumber = "2023001", Picture = "https://placehold.co/600x400/3498db/ffffff?text=Ali", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 2, FirstName = "Nour", LastName = "Khaled", DateOfBirth = new DateTime(2004, 3, 22), IdNumber = "2023002", Picture = "https://placehold.co/600x400/e74c3c/ffffff?text=Nour", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 3, FirstName = "Youssef", LastName = "Mahmoud", DateOfBirth = new DateTime(2005, 5, 10), IdNumber = "2023003", Picture = "https://placehold.co/600x400/2ecc71/ffffff?text=Youssef", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 4, FirstName = "Fatima", LastName = "Said", DateOfBirth = new DateTime(2004, 7, 18), IdNumber = "2023004", Picture = "https://placehold.co/600x400/f1c40f/ffffff?text=Fatima", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 5, FirstName = "Omar", LastName = "Adel", DateOfBirth = new DateTime(2005, 9, 5), IdNumber = "2023005", Picture = "https://placehold.co/600x400/9b59b6/ffffff?text=Omar", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 6, FirstName = "Mariam", LastName = "Gamal", DateOfBirth = new DateTime(2004, 11, 30), IdNumber = "2023006", Picture = "https://placehold.co/600x400/1abc9c/ffffff?text=Mariam", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 7, FirstName = "Ahmed", LastName = "Tarek", DateOfBirth = new DateTime(2005, 2, 20), IdNumber = "2023007", Picture = "https://placehold.co/600x400/e67e22/ffffff?text=Ahmed", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 8, FirstName = "Hana", LastName = "Ibrahim", DateOfBirth = new DateTime(2004, 4, 12), IdNumber = "2023008", Picture = "https://placehold.co/600x400/34495e/ffffff?text=Hana", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 9, FirstName = "Khaled", LastName = "Ezzat", DateOfBirth = new DateTime(2005, 6, 25), IdNumber = "2023009", Picture = "https://placehold.co/600x400/d35400/ffffff?text=Khaled", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Student { Id = 10, FirstName = "Salma", LastName = "Mostafa", DateOfBirth = new DateTime(2004, 8, 8), IdNumber = "2023010", Picture = "https://placehold.co/600x400/c0392b/ffffff?text=Salma", CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser }
        );
    }
}
