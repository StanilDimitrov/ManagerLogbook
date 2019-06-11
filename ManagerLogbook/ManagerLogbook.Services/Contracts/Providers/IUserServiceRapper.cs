using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts.Providers
{
    public interface IUserServiceRapper
    {
       Task<IList<User>> GetAllUsersInRoleAsync(string role);    
    }
}
