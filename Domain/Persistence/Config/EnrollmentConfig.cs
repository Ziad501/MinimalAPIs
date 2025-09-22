using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Persistence.Config;

public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        var seedDate = new DateTime(2025, 9, 20, 16, 53, 0, DateTimeKind.Utc);
        const string seedUser = "System";
        builder.HasData(
            new Enrollment { Id = 1, StudentId = 1, CourseId = 1, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 2, StudentId = 1, CourseId = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 3, StudentId = 2, CourseId = 1, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 4, StudentId = 2, CourseId = 2, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 5, StudentId = 3, CourseId = 4, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 6, StudentId = 4, CourseId = 5, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 7, StudentId = 4, CourseId = 6, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 8, StudentId = 5, CourseId = 2, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 9, StudentId = 6, CourseId = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 10, StudentId = 7, CourseId = 4, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 11, StudentId = 8, CourseId = 1, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 12, StudentId = 9, CourseId = 5, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Enrollment { Id = 13, StudentId = 10, CourseId = 6, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser }
        );
    }
}
