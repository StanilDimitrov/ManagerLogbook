using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelpersUsersController
    {
        public static UserViewModel TestUserViewModel1()
        {
            return new UserViewModel
            {
                Id = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                UserName = "Peter"
            };
        }

        public static UserDTO TestUserDTO4()
        {
            return new UserDTO
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
                CurrentLogbookId = 2
            };
        }


        public static UserManager<User> GetUserManager(ManagerLogbookContext context)
        {
            var store = new UserStore<User>(context);
            var userManager = new UserManager<User>(store, null, null, null, null, null, null, null, null);
            return userManager;
        }
    }
}
