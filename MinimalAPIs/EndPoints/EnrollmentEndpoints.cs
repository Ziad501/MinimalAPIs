using Domain.DTOs.EnrollmentDTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using MinimalAPIs.IRepository;
namespace MinimalAPIs.EndPoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (IGenericRepository<Enrollment> _repo, CancellationToken ct) =>
        {
            return await _repo.Query().Select(p => new EnrollmentDto(p.Id, p.CourseId, p.StudentId)).ToListAsync(ct);
        })
          .WithName("GetAllEnrollments")
        .Produces<List<EnrollmentDto>>(StatusCodes.Status200OK)
        .WithOpenApi(op =>
        {
            op.Summary = "List all enrollments";
            op.Description = "Returns enrollment records as DTOs (Id, CourseId, StudentId).";
            return op.SetJsonExample("200", new OpenApiArray
            {
                new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(9001),
                    ["courseId"] = new OpenApiInteger(101),
                    ["studentId"] = new OpenApiInteger(501)
                }
            });
        });

        group.MapGet("/{id}", async (int id, IGenericRepository<Enrollment> _repo, CancellationToken token) =>
        {
            var dto = await _repo.Query()
                .Where(p => p.Id == id)
                .Select(p => new EnrollmentDto(p.Id, p.CourseId, p.StudentId))
                .FirstOrDefaultAsync(token);

            return dto is null ? Results.NotFound() : Results.Ok(dto);
        })
        .WithName("GetEnrollmentById")
        .Produces<EnrollmentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Get an enrollment by ID";
            op.Description = "Fetches a single enrollment record by ID.";
            op.SetIdDescription("Enrollment identifier (integer > 0).");
            return op.SetJsonExample("200", new OpenApiObject
            {
                ["id"] = new OpenApiInteger(9001),
                ["courseId"] = new OpenApiInteger(101),
                ["studentId"] = new OpenApiInteger(501)
            });
        });

        group.MapPost("/", async (EnrollmentCreateDto enrollmentDto, IGenericRepository<Enrollment> _repo, CancellationToken ct) =>
        {
            var enrollment = new Enrollment
            {
                CourseId = enrollmentDto.CourseId,
                StudentId = enrollmentDto.StudentId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "sys",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "sys"
            };
            await _repo.AddAsync(enrollment, ct);
            return Results.Created($"/api/Enrollment/{enrollment.Id}", enrollmentDto);
        })
        .WithName("CreateEnrollment")
        .Accepts<EnrollmentCreateDto>("application/json")
        .Produces<EnrollmentCreateDto>(StatusCodes.Status201Created)
        .WithOpenApi(op =>
        {
            op.Summary = "Create an enrollment";
            op.Description = "Creates the relation between a student and a course. Response echoes the input payload and sets `Location` header.";
            op.EnsureResponseHeader("201", "Location", "URL of created enrollment", "/api/Enrollment/9001");
            return op.SetJsonExample("201", new OpenApiObject
            {
                ["courseId"] = new OpenApiInteger(101),
                ["studentId"] = new OpenApiInteger(501)
            });
        });

        group.MapPut("/{id}", async (int id, EnrollmentDto enrollment, IGenericRepository<Enrollment> _repo, CancellationToken token) =>
        {
            var affected = await _repo.UpdateAsync(p => p.Id == id,
                setters => setters
                    .SetProperty(m => m.CourseId, enrollment.CourseId)
                    .SetProperty(m => m.StudentId, enrollment.StudentId)
                    .SetProperty(m => m.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(m => m.UpdatedBy, "Sys"), token);

            return affected == 1 ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateEnrollment")
        .Accepts<EnrollmentDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Update an enrollment";
            op.Description = "Full update by ID using set-based SQL. Returns 204 on success.";
            return op.SetIdDescription("Enrollment identifier (integer > 0).");
        });

        group.MapDelete("/{id}", async (int id, IGenericRepository<Enrollment> _repo, CancellationToken ct) =>
        {
            var affected = await _repo.DeleteByIdAsync(id, ct);
            return affected == 1 ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteEnrollment")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Delete an enrollment";
            op.Description = "Permanently removes an enrollment by ID.";
            return op.SetIdDescription("Enrollment identifier (integer > 0).");
        });
    }
}