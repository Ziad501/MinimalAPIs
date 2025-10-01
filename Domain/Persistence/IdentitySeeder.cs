using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Persistence;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        string[] roles = ["Admin", "User"];
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        var adminEmail = "universityadmin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new User
            {
                UserName = "universityadmin",
                Email = adminEmail,
                FirstName = "University",
                LastName = "Admin",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var userEmail = "universityuser@example.com";
        var normalUser = await userManager.FindByEmailAsync(userEmail);
        if (normalUser is null)
        {
            normalUser = new User
            {
                UserName = "universityuser",
                Email = userEmail,
                FirstName = "University",
                LastName = "User",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(normalUser, "User123!");
            await userManager.AddToRoleAsync(normalUser, "User");
        }
    }
}
