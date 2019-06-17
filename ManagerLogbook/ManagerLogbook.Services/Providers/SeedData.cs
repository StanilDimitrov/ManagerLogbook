using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ManagerLogbook.Services.Providers
{
    public class SeedData
    {
        public static async Task SeedDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ManagerLogbookContext>();

                if (dbContext.Roles.Any(u => u.Name == "Admin"))
                {
                    return;
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });

                var adminUser = new User { UserName = "Admin", Email = "admin@admin.bg" };
                await userManager.CreateAsync(adminUser, "123456");

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
