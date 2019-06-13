using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Controllers
{
    [Authorize(Roles = "Admin, Manager, Moderator")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.userService.GetUserByIdAsync(id);
            var model = new IndexUserViewModel();
            model.User = user.MapFrom();

            return View(model);
        }
    }
}