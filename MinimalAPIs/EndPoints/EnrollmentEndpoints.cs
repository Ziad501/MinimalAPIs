using Domain.DTOs.EnrollmentDTOs;
using Domain.Entities;
using Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.IRepository;
using MinimalAPIs.OpenApiSpecs;

namespace MinimalAPIs.EndPoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment")
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

        group.MapPost("/", [Authorize(Roles = "Admin")] async Task<Created<EnrollmentDto>> (
            EnrollmentCreateDto dto,
            IGenericRepository<Enrollment> _repo,
            CancellationToken ct) =>
        {
            var model = new Enrollment
            {
                CourseId = dto.CourseId,
                StudentId = dto.StudentId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "sys",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "sys"
            };

            await _repo.AddAsync(model, ct);

            var resultDto = new EnrollmentDto(model.Id, model.CourseId, model.StudentId);
            return TypedResults.Created($"/api/Enrollment/{model.Id}", resultDto);
        })
        .WithName("CreateEnrollment")
        .Accepts<EnrollmentCreateDto>("application/json")
        .Produces<EnrollmentDto>(StatusCodes.Status201Created)
        .WithOpenApi(EnrollmentsSpecs.Create);

        group.MapPut("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound, BadRequest<string>>> (
            int id,
            EnrollmentDto dto,
            IGenericRepository<Enrollment> _repo,
            CancellationToken ct) =>
        {
            if (id != dto.Id)
                return TypedResults.BadRequest("Route ID and body ID do not match.");

            var affected = await _repo.UpdateAsync(e => e.Id == id,
                setters => setters
                    .SetProperty(e => e.CourseId, dto.CourseId)
                    .SetProperty(e => e.StudentId, dto.StudentId)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(e => e.UpdatedBy, "sys"),
                ct);

            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("UpdateEnrollment")
        .Accepts<EnrollmentDto>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
        .WithOpenApi(EnrollmentsSpecs.Update);

        group.MapDelete("/{id}", [Authorize(Roles = "Admin")] async Task<Results<NoContent, NotFound>> (
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
        .WithOpenApi(EnrollmentsSpecs.Delete);
    }
}
