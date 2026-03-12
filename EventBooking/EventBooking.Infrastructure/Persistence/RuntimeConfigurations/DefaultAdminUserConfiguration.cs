using EventBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Persistence.RuntimeConfigurations
{
    public class DefaultAdminUserConfiguration
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Manager",
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
