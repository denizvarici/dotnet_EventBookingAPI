using EventBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Persistence.RuntimeConfigurations
{
    public class DefaultNormalUserConfiguration
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userEmail = "user@user.com";
            var userUser = await userManager.FindByEmailAsync(userEmail);

            if (userUser == null)
            {
                var newUser = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "Classic User",
                };

                var result = await userManager.CreateAsync(newUser, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "User");
                }
            }

            var userEmail2 = "user2@user.com";
            var userUser2 = await userManager.FindByEmailAsync(userEmail2);

            if (userUser2 == null)
            {
                var newUser2 = new AppUser
                {
                    UserName = userEmail2,
                    Email = userEmail2,
                    FullName = "Classic User 2",
                };

                var result2 = await userManager.CreateAsync(newUser2, "User123!");

                if (result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser2, "User");
                }
            }
        }
    }
}
