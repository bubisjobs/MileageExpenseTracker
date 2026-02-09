using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.Helpers
{
    public static class IdentitySeeder
    {
        //public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        //{
        //    using var scope = serviceProvider.CreateScope();

        //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    // 1. Ensure roles exist
        //    string[] roles =
        //    {
        //        SD.Roles.Admin,
        //        SD.Roles.TeamLead,
        //        SD.Roles.Finance,
        //        SD.Roles.Employees
        //    };

        //    foreach (var role in roles)
        //    {
        //        if (!await roleManager.RoleExistsAsync(role))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(role));
        //        }
        //    }

        //    // 2. Seed Admin user
        //    await SeedUserAsync(
        //        userManager,
        //        email: "admin@bbi.com",
        //        firstName: "System",
        //        lastName: "Admin",
        //        role: SD.Roles.Admin,
        //        password: "Password@123"
        //    );

          
        //}

        //private static async Task SeedUserAsync(
        //    UserManager<User> userManager,
        //    string email,
        //    string firstName,
        //    string lastName,
        //    string role,
        //    string password
        //)
        //{
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user != null) return;

        //    user = new User
        //    {
        //        UserName = email,
        //        Email = email,
        //        FirstName = firstName,
        //        LastName = lastName,
        //        Role = SD.Roles.Admin,
        //        EmailConfirmed = true,
        //        IsActive = true,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    var result = await userManager.CreateAsync(user, password);
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        //    }

        //    await userManager.AddToRoleAsync(user, role);
        //}
    }
}
