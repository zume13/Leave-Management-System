using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Shared;

namespace LeaveManagement.Infrastructure.Identity
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider sp)
        {
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<User>>();
            var _config = sp.GetRequiredService<IConfiguration>();

            string[] roles = { "Admin", "Employee", "Manager" };

            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = _config["AdminCreds:Email"];
            var adminUser = await userManager.FindByEmailAsync(adminEmail!)
                ?? new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmployeeName = "Admin",
                    isEmailVerified = true,
                    verificationToken = null,
                    tokenExpiration = null
                };

            var result = await userManager.CreateAsync(adminUser, _config["AdminCreds:Password"]!);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var addRole = await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!addRole.Succeeded)
                throw new Exception(string.Join(", ", addRole.Errors.Select(e => e.Description)));
        }

    }
}
