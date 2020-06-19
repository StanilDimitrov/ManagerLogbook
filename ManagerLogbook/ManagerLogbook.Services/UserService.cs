using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext _context;
        private readonly IUserServiceWrapper _userWrapper;

        public UserService(ManagerLogbookContext context,
                          IUserServiceWrapper userWrapper)
        {
            _context = context;
            _userWrapper = userWrapper;
        }

        public async Task<UserDTO> GetUserDtoAsync(string userId)
        {
            var user = await _context.Users.Include(x => x.BusinessUnit)
                                               .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            return user.ToDTO();
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            return user;
        }

        public async Task<UserDTO> SwitchLogbookAsync(User user, int logbookId)
        {
            user.CurrentLogbookId = logbookId;

            await _context.SaveChangesAsync();
            return user.ToDTO();
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsNotPresentInBusinessUnitAsync(int businessUnitId)
        {
            var usersOfRoleModerator = await _userWrapper.GetAllUsersInRoleAsync("Moderator");

            var moderators = usersOfRoleModerator
                            .Where(x => x.BusinessUnitId != businessUnitId)
                            .Select(x => x.ToDTO())
                            .ToList();

            return moderators;
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsPresentInBusinessUnitAsync(int businessUnitId)
        {
            var usersOfRoleModerator = await _userWrapper.GetAllUsersInRoleAsync("Moderator");

            var moderators = usersOfRoleModerator.Where(x => x.BusinessUnitId == businessUnitId)
                                                  .Select(x => x.ToDTO())
                                                  .ToList();
            return moderators;
        }

        public async Task<IReadOnlyCollection<UserDTO>> GetAllManagersNotPresentInLogbookAsync(int logbookId)
        {
            var usersOfRoleManager = await _userWrapper.GetAllUsersInRoleAsync("Manager");

            var managersInLogbook = await _context.UsersLogbooks
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
            var managersInLogbook = await _context.UsersLogbooks
                            .Where(x => x.LogbookId == logbookId)
                            .Select(x => x.User.ToDTO())
                            .ToListAsync();

            return managersInLogbook;
        }

        public async Task<bool> CheckIfUserIsManagerOfLogbook(string userId, int logbookId)
        {
            return await _context.UsersLogbooks.AnyAsync(x => x.UserId == userId && x.LogbookId == logbookId);
        }
    }
}
