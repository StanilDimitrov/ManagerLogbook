using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class UserService : IUserService
    {
        private readonly ManagerLogbookContext contex;

        public UserService(ManagerLogbookContext contex)
        {
            this.contex = contex ?? throw new ArgumentNullException(nameof(contex));
        }

        //public async Task<User> EditUser(User user, string email, string picture)
        //{





        //    return user;
        //}
    }
}
