using Domain.DTOs.AuthenticationDtos;
using Domain.DTOs.EnrollmentDTOs;
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

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/enrollments")
                          .WithTags(nameof(Enrollment))
                          .WithGroupName("enrollments");

        group.MapGet("/", async Task<Ok<PagedResult<EnrollmentDto>>> (
            IGenericRepository<Enrollment> _repo,
            int pageNumber = PagingDefaults.DefaultPageNumber,
            int pageSize = PagingDefaults.DefaultPageSize,
            CancellationToken ct = default) =>
        {
            var query = _repo.Query()
                .OrderBy(e => e.Id)
                .Select(e => new EnrollmentDto(e.Id, e.CourseId, e.StudentId));

            var result = await query.ToPagedResultAsync(pageNumber, pageSize, ct);
            return TypedResults.Ok(result);
        })
        .WithName("GetAllEnrollments")
        .Produces<PagedResult<EnrollmentDto>>(StatusCodes.Status200OK)
        .WithOpenApi(EnrollmentsSpecs.List);

        group.MapGet("/{id}", async Task<Results<Ok<EnrollmentDto>, NotFound>> (
            int id,
            IGenericRepository<Enrollment> _repo,
            CancellationToken ct) =>
        {
            var dto = await _repo.Query()
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentDto(e.Id, e.CourseId, e.StudentId))
                .FirstOrDefaultAsync(ct);

            return dto is null ? TypedResults.NotFound() : TypedResults.Ok(dto);
        })
        .WithName("GetEnrollmentById")
        .Produces<EnrollmentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(EnrollmentsSpecs.GetById);

        group.MapPost("/", async Task<Results<Created<EnrollmentDto>, BadRequest<List<ErrorResponseDto>>>> (
            IValidator<EnrollmentCreateDto> validator,
            EnrollmentCreateDto dto,
            IGenericRepository<Enrollment> _repo,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ToErrorsAsync(dto, ct);
            if (validationResult is not null) return TypedResults.BadRequest(validationResult);
            var actor = user.Identity?.Name
                       ?? user.FindFirst("userId")?.Value
                       ?? "system";
            var model = new Enrollment
            {
                CourseId = dto.CourseId,
                StudentId = dto.StudentId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = actor,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = actor
            };

            await _repo.AddAsync(model, ct);

            var resultDto = new EnrollmentDto(model.Id, model.CourseId, model.StudentId);
            return TypedResults.Created($"/api/enrollments/{model.Id}", resultDto);
        })
        .WithName("CreateEnrollment")
        .Accepts<EnrollmentCreateDto>("application/json")
        .Produces<EnrollmentDto>(StatusCodes.Status201Created)
        .WithOpenApi(EnrollmentsSpecs.Create)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound, BadRequest<string>, BadRequest<List<ErrorResponseDto>>>> (
            IValidator<EnrollmentDto> validator,
            int id,
            EnrollmentDto dto,
            IGenericRepository<Enrollment> _repo,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ToErrorsAsync(dto, ct);
            if (validationResult is not null) return TypedResults.BadRequest(validationResult);
            if (id != dto.Id)
                return TypedResults.BadRequest("Route ID and body ID do not match.");
            var actor = user.Identity?.Name
                       ?? user.FindFirst("userId")?.Value
                       ?? "system";
            var affected = await _repo.UpdateAsync(e => e.Id == id,
                setters => setters
                    .SetProperty(e => e.CourseId, dto.CourseId)
                    .SetProperty(e => e.StudentId, dto.StudentId)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(e => e.UpdatedBy, actor),
                ct);

            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("UpdateEnrollment")
        .Accepts<EnrollmentDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
        .WithOpenApi(EnrollmentsSpecs.Update)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (
            int id,
            IGenericRepository<Enrollment> _repo,
            CancellationToken ct) =>
        {
            var affected = await _repo.DeleteByIdAsync(id, ct);
            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteEnrollment")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(EnrollmentsSpecs.Delete)
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

    }
}
