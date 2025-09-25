using Domain.DTOs.CourseDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using MinimalAPIs.IRepository;

namespace MinimalAPIs.EndPoints;
public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/courses").WithTags(nameof(Course));

        group.MapGet("/", async Task<Ok<List<CourseDto>>> (IGenericRepository<Course> _repo, CancellationToken ct) =>
        {
            var courses = await _repo.Query()
                .Select(c => new CourseDto(c.Id, c.Title, c.Credits))
                .ToListAsync(ct);
            return TypedResults.Ok(courses);
        })
        .WithName("GetAllCourses")
        .Produces<List<CourseDto>>(StatusCodes.Status200OK)
        .WithOpenApi(op =>
        {
            op.Summary = "List all courses";
            op.Description = "Returns all courses as lightweight DTOs using server-side projection.";
            return op.SetJsonExample("200", new OpenApiArray
            {
                new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(101),
                    ["title"] = new OpenApiString("Intro to Databases"),
                    ["credits"] = new OpenApiInteger(3)
                }
            });
        });

        group.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound<string>>> (IGenericRepository<Course> _repo, int id, CancellationToken ct) =>
        {
            var dto = await _repo.Query()
                .Where(p => p.Id == id)
                .Select(c => new CourseDto(c.Id, c.Title, c.Credits))
                .FirstOrDefaultAsync(ct);
            return dto is null ? TypedResults.NotFound("course not found!") : TypedResults.Ok(dto);
        })
        .WithName("GetCourseByID")
        .Produces<CourseDto>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status404NotFound, "text/plain")
        .WithOpenApi(op =>
        {
            op.Summary = "Get a course by ID";
            op.Description = "Fetches a single course. Returns 404 (text/plain) if the course does not exist.";
            op.SetIdDescription("Course identifier (integer > 0).");
            return op.SetJsonExample("200", new OpenApiObject
            {
                ["id"] = new OpenApiInteger(101),
                ["title"] = new OpenApiString("Intro to Databases"),
                ["credits"] = new OpenApiInteger(3)
            });
        });

        group.MapPost("/", async Task<Results<Created<CourseDto>, BadRequest>> (IGenericRepository<Course> _repo, CourseCreateDto dto, CancellationToken ct) =>
        {
            var course = new Course
            {
                Title = dto.Title,
                Credits = dto.Credits,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "sys",
                UpdatedBy = "sys"
            };
            await _repo.AddAsync(course, ct);
            var courseDto = new CourseDto(course.Id, course.Title, course.Credits);
            return TypedResults.Created($"/courses/{course.Id}", courseDto);
        })
        .WithName("CreateCourse")
        .Accepts<CourseCreateDto>("application/json")
        .Produces<CourseDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi(op =>
        {
            op.Summary = "Create a course";
            op.Description = "Creates a course and returns the created resource as CourseDto. **Note:** Location header uses `/courses/{id}` (without `/api`).";
            op.EnsureResponseHeader("201", "Location", "Relative URL of created resource", "/courses/123");
            return op.SetJsonExample("201", new OpenApiObject
            {
                ["id"] = new OpenApiInteger(123),
                ["title"] = new OpenApiString("Discrete Math"),
                ["credits"] = new OpenApiInteger(3)
            });
        });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound<string>, BadRequest<string>>> (IGenericRepository<Course> _repo, int id, CourseDto dto, CancellationToken ct) =>
        {
            if (id != dto.Id) return TypedResults.BadRequest("Route ID and course ID do not match.");

            return await _repo.UpdateAsync(p => p.Id == id,
                set => set
                    .SetProperty(x => x.Title, dto.Title)
                    .SetProperty(x => x.Credits, dto.Credits)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedBy, "sys"), ct) == 1 ? TypedResults.NoContent() : TypedResults.NotFound($"course with Id: {id} is misssing");
        })
        .WithName("UpdateCourse")
        .Accepts<CourseDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
        .Produces<string>(StatusCodes.Status404NotFound, "text/plain")
        .WithOpenApi(op =>
        {
            op.Summary = "Update a course";
            op.Description = "Full update by ID using set-based SQL (`ExecuteUpdateAsync`). Returns 204 on success.";
            return op.SetIdDescription("Course identifier (must match body.id).");
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, IGenericRepository<Course> _repo, CancellationToken ct)
            => await _repo.DeleteByIdAsync(id, ct) == 1 ? TypedResults.NoContent() : TypedResults.NotFound())
        .WithName("DeleteCourse")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(op =>
        {
            op.Summary = "Delete a course";
            op.Description = "Permanently removes a course by ID using set-based delete.";
            return op.SetIdDescription("Course identifier (integer > 0).");
        });
    }
}