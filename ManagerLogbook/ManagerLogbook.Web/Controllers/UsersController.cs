using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    [Authorize(Roles = "Admin, Manager, Moderator")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private static readonly ILog log =
        LogManager.GetLogger(typeof(UsersController));

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var user = await this.userService.GetUserByIdAsync(id);
                var model = new IndexUserViewModel();
                model.User = user.MapFrom();

                return View(model);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}