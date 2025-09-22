using Domain.DTOs.CourseDTOs;
using Domain.Entities;
using Domain.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(builder => builder.AddPolicy("AllowMe",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
    ));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMe");

var group = app.MapGroup("/api/courses").WithTags(nameof(Course));
group.MapGet("/", async Task<Ok<List<CourseDto>>> (AppDbContext _context, CancellationToken ct) =>
{
    var courses = await _context.Courses.AsNoTracking()
    .Select(courses => new CourseDto(courses.Id, courses.Title, courses.Credits))
    .ToListAsync(ct);

    return TypedResults.Ok(courses);
})
.WithName("GetAllCourses");

group.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound<string>>> (AppDbContext _context, int id, CancellationToken ct) =>
{
    var course = await _context.Courses.AsNoTracking()
    .Where(p => p.Id == id)
    .Select(course => new CourseDto(course.Id, course.Title, course.Credits))
    .FirstOrDefaultAsync(ct);

    return course is CourseDto dto ? TypedResults.Ok(dto) : TypedResults.NotFound("course not found!");
})
.WithName("GetCourseByID");

group.MapPut("/{id}", async Task<Results<NoContent, NotFound<string>, BadRequest<string>>> (AppDbContext _context, int id, CourseDto dto, CancellationToken ct) =>
{
    if (id != dto.Id) return TypedResults.BadRequest("Route ID and course ID do not match.");
    var course = await _context.Courses.Where(p => p.Id == id)
    .ExecuteUpdateAsync(set => set
    .SetProperty(x => x.Title, dto.Title)
    .SetProperty(x => x.Credits, dto.Credits), ct);

    return course == 1 ? TypedResults.NoContent() : TypedResults.NotFound($"course with Id: {id} is misssing");
});
group.MapPost("/", async Task<Results<Created<CourseDto>, BadRequest>> (AppDbContext _context, CourseCreateDto dto, CancellationToken ct) =>
{
    Course course = new()
    {
        Title = dto.Title,
        Credits = dto.Credits,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        CreatedBy = "sys",
        UpdatedBy = "sys"
    };
    _context.Add(course);
    await _context.SaveChangesAsync(ct);
    var courseDto = new CourseDto(course.Id, dto.Title, dto.Credits);
    return TypedResults.Created($"/courses/{course.Id}", courseDto);
});
group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, AppDbContext _context, CancellationToken ct) =>
{
    var course = await _context.Courses
        .Where(model => model.Id == id)
        .ExecuteDeleteAsync(ct);
    return course == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
})
.WithName("DeleteCourse");

app.MapStudentEndpoints();
app.MapEnrollmentEndpoints();

app.Run();
