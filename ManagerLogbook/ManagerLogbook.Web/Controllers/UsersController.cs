using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await this.userService.GetUserDtoByIdAsync(id);

            var viewModel = new IndexUserViewModel
            {
                User = user.MapFrom()
            };

            return View(viewModel);
        }
    }
}