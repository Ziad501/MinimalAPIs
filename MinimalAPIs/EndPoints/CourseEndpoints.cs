using Domain.DTOs.CourseDTOs;
using Domain.Entities;
using Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.IRepository;
using MinimalAPIs.OpenApiSpecs;

namespace MinimalAPIs.EndPoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/courses")
                          .WithTags(nameof(Course))
                          .WithGroupName("courses");

        // Paged list
        group.MapGet("/", async Task<Ok<PagedResult<CourseDto>>> (IGenericRepository<Course> _repo, int pageNumber = PagingDefaults.DefaultPageNumber, int pageSize = PagingDefaults.DefaultPageSize, CancellationToken ct = default) =>
        {
            var query = _repo.Query()
                .OrderBy(c => c.Id)
                .Select(c => new CourseDto(c.Id, c.Title, c.Credits));
            var result = await query.ToPagedResultAsync(pageNumber, pageSize, ct);
            return TypedResults.Ok(result);
        })
        .WithName("GetAllCourses")
        .Produces<PagedResult<CourseDto>>(StatusCodes.Status200OK)
        .WithOpenApi(CoursesSpecs.List);

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
        .WithOpenApi(CoursesSpecs.GetById);

        group.MapPost("/", [Authorize(Roles = "Admin")] async Task<Results<Created<CourseDto>, BadRequest>> (IGenericRepository<Course> _repo, CourseCreateDto dto, CancellationToken ct) =>
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
        .WithOpenApi(CoursesSpecs.Create);

        group.MapPut("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound<string>, BadRequest<string>>> (IGenericRepository<Course> _repo, int id, CourseDto dto, CancellationToken ct) =>
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
        .WithOpenApi(CoursesSpecs.Update);

        group.MapDelete("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound>> (int id, IGenericRepository<Course> _repo, CancellationToken ct)
            => await _repo.DeleteByIdAsync(id, ct) == 1 ? TypedResults.NoContent() : TypedResults.NotFound())
        .WithName("DeleteCourse")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(CoursesSpecs.Delete);
    }
}
