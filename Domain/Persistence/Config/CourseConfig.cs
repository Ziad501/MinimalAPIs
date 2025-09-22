using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Persistence.Config;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        var seedDate = new DateTime(2025, 9, 20, 16, 53, 0, DateTimeKind.Utc);
        const string seedUser = "System";
        builder.HasData(
            new Course { Id = 1, Title = "Computer Science Fundamentals", Credits = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Course { Id = 2, Title = "Advanced Database Management", Credits = 4, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Course { Id = 3, Title = "Modern Web Applications", Credits = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Course { Id = 4, Title = "Algorithms and Data Structures", Credits = 4, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Course { Id = 5, Title = "Software Engineering Principles", Credits = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser },
            new Course { Id = 6, Title = "Introduction to Artificial Intelligence", Credits = 3, CreatedAt = seedDate, UpdatedAt = seedDate, CreatedBy = seedUser, UpdatedBy = seedUser }
        );
    }
}
