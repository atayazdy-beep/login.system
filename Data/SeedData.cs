using Microsoft.AspNetCore.Identity;
using WasteLogin.Models;

namespace WasteLogin.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services, IHostEnvironment environment)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Demo credentials are created only in Development. Override them with
        // environment variables before sharing or deploying the project.
        if (!environment.IsDevelopment()) return;

        await CreateIfMissing(userManager,
            Environment.GetEnvironmentVariable("SEED_ADMIN_USERNAME") ?? "admin",
            Environment.GetEnvironmentVariable("SEED_ADMIN_EMAIL") ?? "admin@example.com",
            Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD") ?? "Admin@12345",
            "مدیر سامانه", "Admin");

        await CreateIfMissing(userManager,
            Environment.GetEnvironmentVariable("SEED_USER_USERNAME") ?? "user",
            Environment.GetEnvironmentVariable("SEED_USER_EMAIL") ?? "user@example.com",
            Environment.GetEnvironmentVariable("SEED_USER_PASSWORD") ?? "User@12345",
            "کاربر سامانه", "User");
    }

    private static async Task CreateIfMissing(
        UserManager<ApplicationUser> userManager,
        string username, string email, string password, string fullName, string role)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName
            };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    "Could not seed " + role + ": " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    "Could not assign role " + role + ": " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}
