using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using ManagerLogbook.Web.Areas.Manager.Models;
using log4net;
using ManagerLogbook.Services.CustomExeptions;

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LogbooksController : Controller
    {
        private readonly ILogbookService logbookService;
        private readonly IUserService userService;
        private readonly IImageOptimizer optimizer;
        private static readonly ILog log = LogManager.GetLogger(typeof(LogbooksController));

        public LogbooksController(ILogbookService logbookService,
                                  IUserService userService,
                                  IImageOptimizer optimizer)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogbookViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            try
            {
                string imageName = null;

                if (model.LogbookPicture != null)
                {
                    imageName = optimizer.OptimizeImage(model.LogbookPicture, 268, 182);
                }

                var logbook = await this.logbookService.CreateLogbookAsync(model.Name, model.BusinessUnitId, imageName);

                if (logbook.Name == model.Name)
                {
                    return Ok(string.Format(WebConstants.LogbookCreated, model.Name));
                }

                return BadRequest(string.Format(WebConstants.LogbookNotCreated, model.Name));
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }               
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LogbookViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToUpdateLogbook, model.Name));
            }

            try
            {
                var logbookDTO = await this.logbookService.GetLogbookById(model.Id);

                string imageName = null;

                if (model.Picture != null)
                {
                    imageName = optimizer.OptimizeImage(model.LogbookPicture, 268, 182);
                }

                if (model.Picture != null)
                {
                    optimizer.DeleteOldImage(model.Picture);
                }

                logbookDTO = await this.logbookService.UpdateLogbookAsync(model.Id, model.Name, model.BusinessUnitId, imageName);

                if (logbookDTO.Name != model.Name)
                {
                    return BadRequest(string.Format(WebConstants.UnableToUpdateLogbook, model.Name));
                }

                return Ok(string.Format(WebConstants.LogbookUpdated, model.Name));
            }

            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }            
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManagerToLogbook(LogbookViewModel model)
        {
            try
            {
                var managerId = model.ManagerId;
                var logbookId = model.Id;

                var logbook = await this.logbookService.GetLogbookById(logbookId);

                var manager = await this.userService.GetUserByIdAsync(managerId);

                await this.logbookService.AddManagerToLogbookAsync(managerId, logbookId);

                return Ok(string.Format(WebConstants.SuccessfullyAddedManagerToLogbook, manager.UserName, logbook.Name));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveManagerFromLogbook(LogbookViewModel model)
        {
            try
            {
                var logbookId = model.Id;
                var managerId = model.ManagerId;
                

                var logbook = await this.logbookService.GetLogbookById(logbookId);

                var manager = await this.userService.GetUserByIdAsync(managerId);

                await this.logbookService.RemoveManagerFromLogbookAsync(managerId, logbookId);

                return Ok(string.Format(WebConstants.SuccessfullyRemovedManagerFromLogbook, manager.UserName, logbook.Name));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetAllManagersNotPresent(int id)
        {
            var managers = await this.userService.GetAllManagersNotPresentInLogbookAsync(id);

            if (managers == null)
            {
                return BadRequest(string.Format(WebConstants.ManagerNotExist));
            }

            return Json(managers);
        }

        public async Task<IActionResult> GetAllManagersPresent(int id)
        {
            var managers = await this.userService.GetAllManagersPresentInLogbookAsync(id);

            if (managers == null)
            {
                return BadRequest(string.Format(WebConstants.ManagerNotExist));
            }

            return Json(managers);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}