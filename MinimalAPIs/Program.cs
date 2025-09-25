using Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.EndPoints;
using MinimalAPIs.IRepository;
using MinimalAPIs.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(builder => builder.AddPolicy("AllowMe",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
    ));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMe");

app.MapCourseEndpoints();
app.MapStudentEndpoints();
app.MapEnrollmentEndpoints();

app.Run();
