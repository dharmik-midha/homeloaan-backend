using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Advisor", "User" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Add Admin
            var email = "advisor1@email.com";
            var name = "Advisor1";
            var password = "Abcd@1234";

            if (userManager.FindByEmailAsync(email).Result == null)
            {
                Advisor advisor = new Advisor()
                {
                    Email = email,
                    UserName = email,
                    Name = name,
                };

                IdentityResult result = userManager.CreateAsync(advisor, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(advisor, "Advisor").Wait();
                }
            }
        }
    }
}
