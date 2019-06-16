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
using Microsoft.AspNetCore.Identity;
using ManagerLogbook.Data.Models;
using log4net;
using ManagerLogbook.Services.CustomExeptions;

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BusinessUnitsController : Controller
    {
        private readonly IBusinessUnitService businessUnitService;
        private readonly IUserService userService;
        private readonly IImageOptimizer optimizer;
        private static readonly ILog log = LogManager.GetLogger(typeof(BusinessUnitsController));

        public BusinessUnitsController(IBusinessUnitService businessUnitService,
                                      IUserService userService,
                                      IImageOptimizer optimizer)
        {
            this.businessUnitService = businessUnitService;
            this.userService = userService;
            this.optimizer = optimizer;
        }                

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessUnitViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            try
            {
                string imageName = null;

                if (model.BusinessUnitPicture != null)
                {
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 400, 800);
                }

                var businessUnit = await this.businessUnitService.CreateBusinnesUnitAsync(model.Name, model.Address, model.PhoneNumber, model.Email, model.Information, model.CategoryId, model.TownId, imageName);

                if (businessUnit.Name == model.Name)
                {
                    return Ok(string.Format(WebConstants.BusinessUnitCreated, model.Name));
                }

                return BadRequest(string.Format(WebConstants.BusinessUnitNotCreated,model.Name));
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
        public async Task<IActionResult> Update(BusinessUnitViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToUpdateBusinessUnit, model.Name));
            }

            try
            {
                var businessUnitDTO = await this.businessUnitService.GetBusinessUnitById(model.Id);

                string imageName = null;

                if (model.BusinessUnitPicture != null)
                {
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 400, 800);
                }

                if (model.Picture != null)
                {
                    optimizer.DeleteOldImage(model.Picture);
                }

                businessUnitDTO = await this.businessUnitService.UpdateBusinessUnitAsync(model.Id, model.Name, model.Address, model.PhoneNumber, model.Information, model.Email, model.CategoryId, model.TownId, imageName);

                if (businessUnitDTO.Name != model.Name)
                {
                    return BadRequest(string.Format(WebConstants.UnableToUpdateBusinessUnit,model.Name));
                }

                return Ok(string.Format(WebConstants.BusinessUnitUpdated, model.Name));
            }

            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AlreadyExistsException ex)
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
        public async Task<IActionResult> AddModeratorToBusinessUnit(BusinessUnitViewModel model)
        {
            try
            {
                var businessUnitId = model.Id;
                var moderatorId = model.ModeratorId;

                var businessUnit = await this.businessUnitService.GetBusinessUnitById(businessUnitId);

                var moderator = await this.userService.GetUserByIdAsync(moderatorId);

                await this.businessUnitService.AddModeratorToBusinessUnitsAsync(moderatorId, businessUnitId);

                return Ok(string.Format(WebConstants.SuccessfullyAddedModeratorToBusinessUnit, moderator.UserName, businessUnit.Name));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveModerator(BusinessUnitViewModel model)
        {
            try
            {
                var businessUnitId = model.Id;
                var moderatorId = model.ModeratorId;
                

                var businessUnit = await this.businessUnitService.GetBusinessUnitById(businessUnitId);

                var moderator = await this.userService.GetUserByIdAsync(moderatorId);

                await this.businessUnitService.RemoveModeratorFromBusinessUnitsAsync(moderatorId, businessUnitId);

                return Ok(string.Format(WebConstants.SuccessfullyRemovedModeratorFromBusinessUnit, moderator.UserName, businessUnit.Name));
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

        [HttpGet]
        public async Task<IActionResult> GetAllBusinessUnitCategories()
        {
            try
            {
                var categories = await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();

                return Json(categories);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinessUnits()
        {
            try
            {
                var businessUnits = await this.businessUnitService.GetAllBusinessUnitsAsync();

                return Json(businessUnits);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTowns()
        {
            try
            {
                var towns = await this.businessUnitService.GetAllTownsAsync();

                return Json(towns);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetAllModeratorsNotPresent(int id)
        {
            var moderators = await this.userService.GetAllModeratorsNotPresentInBusinessUnitAsync(id);

            if (moderators == null)
            {
                return BadRequest(string.Format(WebConstants.ModeratorNotExist));
            }

            return Json(moderators);
        }

        public async Task<IActionResult> GetAllModeratorsPresent(int id)
        {
            var moderators = await this.userService.GetAllModeratorsPresentInBusinessUnitAsync(id);

            if (moderators == null)
            {
                return BadRequest(string.Format(WebConstants.ModeratorNotExist));
            }

            return Json(moderators);
        }
    }
}