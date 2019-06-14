////using ManagerLogbook.Data.Models;
//using System;
//using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace ManagerLogbook.Web.Services
{
    public class UserRapper
    {
        private readonly UserManager<User> userManager;

        public UserRapper(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public string GetLoggedUserIdAsync(ClaimsPrincipal principles)
        {
            return this.userManager.GetUserId(principles);
        }

    }
}

class StringWrapper
{
    private string str;

    public StringWrapper(string str)
    {
        this.str = str;
    }

    public string Substring(int startIndex)
    {
        return this.str.Substring(startIndex);
    }
}