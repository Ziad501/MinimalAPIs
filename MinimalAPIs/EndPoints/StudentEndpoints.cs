using Domain.DTOs.AuthenticationDtos;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Utility;
using Domain.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.IRepository;
using MinimalAPIs.OpenApiSpecs;
using System.Security.Claims;

namespace MinimalAPIs.EndPoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/students")
                          .WithTags(nameof(Student))
                          .WithGroupName("students");

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

        group.MapPost("/", async Task<Results<
        Created<StudentDto>,
        BadRequest<List<ErrorResponseDto>>>> (
            IValidator<StudentCreateDto> validator,
            StudentCreateDto dto,
            IGenericRepository<Student> _repo,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ToErrorsAsync(dto, ct);
            if (validationResult is not null) return TypedResults.BadRequest(validationResult);
            var actor = user.Identity?.Name
                       ?? user.FindFirst("userId")?.Value
                       ?? "system";

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                IdNumber = dto.IdNumber,
                Picture = dto.Picture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = actor,
                UpdatedBy = actor,
            };
            await _repo.AddAsync(student, ct);

            var studentDto = new StudentDto(student.Id, student.FirstName, student.LastName, student.DateOfBirth, student.IdNumber, student.Picture);

            return TypedResults.Created($"/api/students/{student.Id}", studentDto);
        })
        .WithName("CreateStudent")
        .Accepts<StudentCreateDto>("application/json")
        .Produces<StudentCreateDto>(StatusCodes.Status201Created)
        .WithOpenApi(StudentsSpecs.Create)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }); ;

        group.MapPut("/{id}", async Task<Results<
            NoContent,
            NotFound,
            BadRequest<string>,
            BadRequest<List<ErrorResponseDto>>>> (
            IValidator<StudentDto> validator,
            int id,
            StudentDto student,
            IGenericRepository<Student> _repo,
            ClaimsPrincipal user,
            CancellationToken token) =>
        {
            var validationResult = await validator.ToErrorsAsync(student, token);
            if (validationResult is not null) return TypedResults.BadRequest(validationResult);
            if (id != student.Id) return TypedResults.BadRequest("Route ID and course ID do not match.");

            var actor = user.Identity?.Name
                        ?? user.FindFirst("userId")?.Value
                        ?? "system";

            var affected = await _repo.UpdateAsync(model => model.Id == id,
                setters => setters
                    .SetProperty(m => m.FirstName, student.FirstName)
                    .SetProperty(m => m.LastName, student.LastName)
                    .SetProperty(m => m.DateOfBirth, student.DateOfBirth)
                    .SetProperty(m => m.IdNumber, student.IdNumber)
                    .SetProperty(m => m.Picture, student.Picture)
                    .SetProperty(m => m.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(m => m.UpdatedBy, actor),
                token);

            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("UpdateStudent")
        .Accepts<StudentDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(StudentsSpecs.Update)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (
            int id,
            IGenericRepository<Student> _repo,
            CancellationToken ct) =>
        {
            var affected = await _repo.DeleteByIdAsync(id, ct);
            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(StudentsSpecs.Delete)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}
