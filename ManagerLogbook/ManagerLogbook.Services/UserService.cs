using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext context;
        private readonly IUserServiceWrapper userRapper;

        public UserService(ManagerLogbookContext context,
                          IUserServiceWrapper userRapper)
        {
            this.context = context;
            this.userRapper = userRapper;
        }

        public async Task<UserDTO> GetUserDtoByIdAsync(string userId)
        {
            var user = await this.context.Users.Include(x => x.BusinessUnit)
                                               .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            return user.ToDTO();
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            return user;
        }

        public async Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId)
        {
            var user = await this.context.Users
                                         .Include(x => x.BusinessUnit)
                                         .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            var logbook = await this.context.Logbooks.FindAsync(logbookId);
            if (logbook == null)
            {
                throw new NotFoundException(ServicesConstants.LogbookNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserNotManagerOfLogbook, user.UserName, logbook.Name));
            }

            user.CurrentLogbookId = logbookId;

            await this.context.SaveChangesAsync();
            return user.ToDTO();

        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsNotPresentInBusinessUnitAsync(int businessUnitId)
        {
            var usersOfRoleModerator = await this.userRapper.GetAllUsersInRoleAsync("Moderator");

            var moderators = usersOfRoleModerator
                            .Where(x => x.BusinessUnitId != businessUnitId)
                            .Select(x => x.ToDTO())
                            .ToList();

            return moderators;
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsPresentInBusinessUnitAsync(int businessUnitId)
        {
            var usersOfRoleModerator = await this.userRapper.GetAllUsersInRoleAsync("Moderator");

            var moderators = usersOfRoleModerator.Where(x => x.BusinessUnitId == businessUnitId)
                                                  .Select(x => x.ToDTO())
                                                  .ToList();
            return moderators;
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllManagersNotPresentInLogbookAsync(int logbookId)
        {
            var usersOfRoleManager = await this.userRapper.GetAllUsersInRoleAsync("Manager");

            var managersInLogbook = await this.context.UsersLogbooks
                                                          .Where(x => x.LogbookId == logbookId)
                                                          .Select(x => x.User)
                                                          .ToListAsync();

            var managersNotInLogbook = usersOfRoleManager.Except(managersInLogbook)
                                                         .Select(x => x.ToDTO())
                                                         .ToList();

            return managersNotInLogbook;
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllManagersPresentInLogbookAsync(int logbookId)
        {
            var managersInLogbook = await this.context.UsersLogbooks
                            .Where(x => x.LogbookId == logbookId)
                            .Select(x => x.User.ToDTO())
                            .ToListAsync();

            return managersInLogbook;
        }
    }
}
