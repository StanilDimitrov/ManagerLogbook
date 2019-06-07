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

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext context;

        public UserService(ManagerLogbookContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
            var user = await this.context.Users.Include(x => x.BusinessUnit)
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

        public async Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsAsync()
        {
            
            return null;
        }
    }
}
