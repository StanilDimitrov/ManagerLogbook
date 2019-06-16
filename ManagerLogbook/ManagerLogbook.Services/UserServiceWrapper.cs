using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class UserServiceWapper : IUserServiceWrapper
    {
        private readonly UserManager<User> userManager;

        public UserServiceWapper(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IList<User>> GetAllUsersInRoleAsync(string role)
        {
            var usersInRole = await this.userManager.GetUsersInRoleAsync(role);
            return usersInRole;
        }

        public string GetLoggedUserId(ClaimsPrincipal principles)
        {
            var userId = this.userManager.GetUserId(principles);
            if (userId == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }
            return userId;
        }


    }
}
