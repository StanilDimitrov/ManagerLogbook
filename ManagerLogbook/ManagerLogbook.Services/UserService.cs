using ManagerLogbook.Data;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext context;

        public UserService(ManagerLogbookContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var user = await this.context.Users.Include(x => x.BusinessUnit)
                                               .FirstOrDefaultAsync(x => x.Id == userId);

            return user.ToDTO();
        }
       
    }
}
