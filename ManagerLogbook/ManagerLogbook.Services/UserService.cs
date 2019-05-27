using ManagerLogbook.Data;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using System.Threading.Tasks;
using System;

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
            var user = await this.context.Users.FindAsync(userId);

            return user.ToDTO();
        }
    }
}
