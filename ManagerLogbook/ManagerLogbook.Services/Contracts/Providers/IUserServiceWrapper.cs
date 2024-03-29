﻿using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts.Providers
{
    public interface IUserServiceWrapper
    {
       Task<IList<User>> GetAllUsersInRoleAsync(string role);

       string GetLoggedUserId(ClaimsPrincipal principles);
    }
}
