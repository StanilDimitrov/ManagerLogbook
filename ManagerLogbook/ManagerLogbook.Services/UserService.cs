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

            //if (user == null)
            //{

            //    //CUSTOM EXCEPTION
            //    throw new Exception();
            //}

            return user.ToDTO();
        }

        public async Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId)
        {
            var user = await this.context.Users.Include(x => x.BusinessUnit)
                                             .FirstOrDefaultAsync(x => x.Id == userId);

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserNotFromLogbook);
            }

            user.CurrentLogbookId = logbookId;

            await this.context.SaveChangesAsync();

            return user.ToDTO();

        }
   
    }
}
