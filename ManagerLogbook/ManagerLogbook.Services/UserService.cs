using ManagerLogbook.Data;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Identity;
using ManagerLogbook.Services.Contracts.Providers;

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext context;
        private readonly IUserServiceRapper userRapper;

        public UserService(ManagerLogbookContext context,
                          IUserServiceRapper userRapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userRapper = userRapper ?? throw new ArgumentNullException(nameof(userRapper));
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await this.context.Users.Include(x => x.BusinessUnit)
                                               .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            return user.ToDTO();
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
                                                          .Where(x => x.LogbookId != logbookId)
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
