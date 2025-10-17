using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.IRepository;
using MinimalAPIs.OpenApiSpecs;

namespace MinimalAPIs.EndPoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student")
                          .WithTags(nameof(Student))
                          .WithGroupName("students");

        // Paged list
        group.MapGet("/", async Task<Ok<PagedResult<StudentDto>>> (
            IGenericRepository<Student> _repo,
            int pageNumber = PagingDefaults.DefaultPageNumber,
            int pageSize = PagingDefaults.DefaultPageSize,
            CancellationToken ct = default) =>
        {
            var query = _repo.Query()
                .OrderBy(s => s.Id)
                .Select(p => new StudentDto(p.Id, p.FirstName, p.LastName, p.DateOfBirth, p.IdNumber, p.Picture));
            var result = await query.ToPagedResultAsync(pageNumber, pageSize, ct);
            return TypedResults.Ok(result);
        })
        .WithName("GetAllStudents")
        .Produces<PagedResult<StudentDto>>(StatusCodes.Status200OK)
        .WithOpenApi(StudentsSpecs.List);

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
        .WithOpenApi(StudentsSpecs.GetById);

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
        .WithOpenApi(StudentsSpecs.Create);

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
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(StudentsSpecs.Update);

        group.MapDelete("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound>> (int id, IGenericRepository<Student> _repo, CancellationToken ct) =>
        {
            var affected = await _repo.DeleteByIdAsync(id, ct);
            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(StudentsSpecs.Delete);
    }
}
