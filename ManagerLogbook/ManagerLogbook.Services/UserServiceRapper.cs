using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts.Providers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class UserServiceRapper : IUserServiceRapper
    {
        private readonly UserManager<User> userManager;

        public UserServiceRapper(UserManager<User> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IList<User>> GetAllUsersInRoleAsync(string role)
        {
            var usersInRole = await this.userManager.GetUsersInRoleAsync(role);
            return usersInRole;
        }

        
    }
}
