using Domain.Entities;
using Domain.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIs.EndPoints;
using MinimalAPIs.IRepository;
using MinimalAPIs.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger with multiple documents
builder.Services.AddEndpointsApiExplorer();
// Program.cs
builder.Services.AddSwaggerGen(c =>
{
    // catch-all default so /swagger/v1/swagger.json resolves
    c.SwaggerDoc("v1", new() { Title = "MinimalAPIs", Version = "v1" });

    // your split docs
    c.SwaggerDoc("auth", new() { Title = "Authentication API", Version = "v1" });
    c.SwaggerDoc("students", new() { Title = "Students API", Version = "v1" });
    c.SwaggerDoc("courses", new() { Title = "Courses API", Version = "v1" });
    c.SwaggerDoc("enrollments", new() { Title = "Enrollments API", Version = "v1" });

    // include everything in "v1", and filter others by GroupName
    c.DocInclusionPredicate((doc, api) =>
        doc == "v1" || string.Equals(api.GroupName, doc, StringComparison.OrdinalIgnoreCase));
});



builder.Services.AddCors(b => b.AddPolicy("AllowMe", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager();

builder.Services.AddScoped<MinimalAPIs.Services.IAuthManager, MinimalAPIs.Services.AuthManager>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalAPIs");   // default
        c.SwaggerEndpoint("/swagger/auth/swagger.json", "Authentication");
        c.SwaggerEndpoint("/swagger/students/swagger.json", "Students");
        c.SwaggerEndpoint("/swagger/courses/swagger.json", "Courses");
        c.SwaggerEndpoint("/swagger/enrollments/swagger.json", "Enrollments");
        c.DefaultModelsExpandDepth(-1);

        // (optional) move UI to a new path to bust cache
        // c.RoutePrefix = "docs";
    });
}



app.UseHttpsRedirection();
app.UseCors("AllowMe");
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints (Minimal APIs)
app.MapAuthenticationEndpoints();
app.MapCourseEndpoints();
app.MapStudentEndpoints();
app.MapEnrollmentEndpoints();

app.Run();
