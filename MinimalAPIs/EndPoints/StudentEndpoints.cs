using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using MinimalAPIs.IRepository;
namespace MinimalAPIs.EndPoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", async Task<Ok<List<StudentDto>>> (IGenericRepository<Student> _repo) =>
        {
            return TypedResults.Ok(
                await _repo.Query()
                .Select(p => new StudentDto(p.Id, p.FirstName, p.LastName, p.DateOfBirth, p.IdNumber, p.Picture))
                .ToListAsync());
        })
        .WithName("GetAllStudents")
        .Produces<List<StudentDto>>(StatusCodes.Status200OK)
        .WithOpenApi(op =>
        {
            op.Summary = "List all students";
            op.Description = "Returns students as DTOs via server-side projection. No tracking is used.";
            return op.SetJsonExample("200", new OpenApiArray
            {
                new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(501),
                    ["firstName"] = new OpenApiString("Sara"),
                    ["lastName"] = new OpenApiString("Ali"),
                    ["dateOfBirth"] = new OpenApiString("2001-05-17"),
                    ["idNumber"] = new OpenApiString("A123456789"),
                    ["picture"] = new OpenApiString("https://example.com/sara.jpg")
                }
            });
        });

        group.MapGet("/{id}", async Task<Results<Ok<StudentDto>, NotFound>> (int id, IGenericRepository<Student> _repo, CancellationToken ct) =>
        {
            var dto = await _repo.Query()
                .Where(p => p.Id == id)
                .Select(p => new StudentDto(p.Id, p.FirstName, p.LastName, p.DateOfBirth, p.IdNumber, p.Picture))
                .FirstOrDefaultAsync(ct);

            return dto is null ? TypedResults.NotFound() : TypedResults.Ok(dto);
        })
        .WithName("GetStudentById")
        .Produces<StudentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Get a student by ID";
            op.Description = "Fetch a single student. Returns 404 if not found.";
            op.SetIdDescription("Student identifier (integer > 0).");
            return op.SetJsonExample("200", new OpenApiObject
            {
                ["id"] = new OpenApiInteger(501),
                ["firstName"] = new OpenApiString("Sara"),
                ["lastName"] = new OpenApiString("Ali"),
                ["dateOfBirth"] = new OpenApiString("2001-05-17"),
                ["idNumber"] = new OpenApiString("A123456789"),
                ["picture"] = new OpenApiString("https://example.com/sara.jpg")
            });
        });

        group.MapPost("/", [Authorize(Roles = "Admin")] async (StudentCreateDto studentDto, IGenericRepository<Student> _repo, CancellationToken ct) =>
        {
            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                DateOfBirth = studentDto.DateOfBirth,
                IdNumber = studentDto.IdNumber,
                Picture = studentDto.Picture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "sys",
                UpdatedBy = "sys",
            };
            await _repo.AddAsync(student, ct);
            return TypedResults.Created($"/api/Student/{student.Id}", studentDto);
        })
        .WithName("CreateStudent")
        .Accepts<StudentCreateDto>("application/json")
        .Produces<StudentCreateDto>(StatusCodes.Status201Created)
        .WithOpenApi(op =>
        {
            op.Summary = "Create a new student";
            op.Description = "Creates a student and returns the submitted payload. Note this response does **not** include the generated id.";
            op.EnsureResponseHeader("201", "Location", "URL of created student", "/api/Student/501");
            return op.SetJsonExample("201", new OpenApiObject
            {
                ["firstName"] = new OpenApiString("Sara"),
                ["lastName"] = new OpenApiString("Ali"),
                ["dateOfBirth"] = new OpenApiString("2001-05-17"),
                ["idNumber"] = new OpenApiString("A123456789"),
                ["picture"] = new OpenApiNull()
            });
        });

        group.MapPut("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound, BadRequest<string>>> (int id, StudentDto student, IGenericRepository<Student> _repo, CancellationToken token) =>
        {
            if (id != student.Id) return TypedResults.BadRequest("Route ID and course ID do not match.");

            var affected = await _repo.UpdateAsync(model => model.Id == id,
                setters => setters
                    .SetProperty(m => m.FirstName, student.FirstName)
                    .SetProperty(m => m.LastName, student.LastName)
                    .SetProperty(m => m.DateOfBirth, student.DateOfBirth)
                    .SetProperty(m => m.IdNumber, student.IdNumber)
                    .SetProperty(m => m.Picture, student.Picture)
                    .SetProperty(m => m.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(m => m.UpdatedBy, "sys"),
                token);

            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("UpdateStudent")
        .Accepts<StudentDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Update a student";
            op.Description = "Full update by ID using set-based SQL. Returns 204 on success.";
            return op.SetIdDescription("Student identifier (must match body.id).");
        });

        group.MapDelete("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound>> (int id, IGenericRepository<Student> _repo, CancellationToken ct) =>
        {
            var affected = await _repo.DeleteByIdAsync(id, ct);
            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Delete a student";
            op.Description = "Permanently removes a student record by ID.";
            return op.SetIdDescription("Student identifier (integer > 0).");
        });
    }
}