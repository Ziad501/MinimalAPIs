using Domain.Entities;
using Domain.Persistence;
using Microsoft.EntityFrameworkCore;
namespace MinimalAPIs.EndPoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (AppDbContext db, CancellationToken token) =>
        {
            return await db.Enrollments.ToListAsync(token);
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi()
        .Produces<List<Enrollment>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, AppDbContext db, CancellationToken token) =>
        {
            return await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id, token)
                is Enrollment model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async (int id, Enrollment enrollment, AppDbContext db, CancellationToken token) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CourseId, enrollment.CourseId)
                    .SetProperty(m => m.StudentId, enrollment.StudentId)
                    .SetProperty(m => m.Id, enrollment.Id)
                    .SetProperty(m => m.CreatedAt, enrollment.CreatedAt)
                    .SetProperty(m => m.UpdatedAt, enrollment.UpdatedAt)
                    .SetProperty(m => m.CreatedBy, enrollment.CreatedBy)
                    .SetProperty(m => m.UpdatedBy, enrollment.UpdatedBy)
                    , token);
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (Enrollment enrollment, AppDbContext db, CancellationToken token) =>
        {
            db.Enrollments.Add(enrollment);
            await db.SaveChangesAsync(token);
            return Results.Created($"/api/Enrollment/{enrollment.Id}", enrollment);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async (int id, AppDbContext db, CancellationToken token) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync(token);
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
