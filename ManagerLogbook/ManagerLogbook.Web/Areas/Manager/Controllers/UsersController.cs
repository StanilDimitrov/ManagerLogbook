using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ILogbookService logbookService;
        private static readonly ILog log =
        LogManager.GetLogger(typeof(UsersController));

        public UsersController(IUserService userService, ILogbookService logbookService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchLogbook(ManagerViewModel model)
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);
                if (model.CurrentLogbookId.HasValue)
                {
                    var logbook = await this.logbookService.GetLogbookById(model.CurrentLogbookId.Value);
                    if (user.CurrentLogbookId ==  model.CurrentLogbookId)
                    {
                        return BadRequest((string.Format(WebConstants.AlreadyInLogbook,  user.UserName, logbook.Name)));
                    }
                    
                    user = await this.userService.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value);
                    return Ok(string.Format(WebConstants.SwitchLogbook, logbook.Name));
                }

                return BadRequest(WebConstants.NoLogbookChoosen);
               
            }

            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
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