using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(BusinessUnitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.BusinessUnitPicture != null)
            {
                imageName = optimizer.OptimizeImage(viewModel.BusinessUnitPicture, 400, 800);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;
            await this.businessUnitService.CreateBusinnesUnitAsync(model);

            return Ok(string.Format(WebConstants.BusinessUnitCreated, viewModel.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BusinessUnitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.BusinessUnitPicture != null)
            {
                imageName = optimizer.OptimizeImage(viewModel.BusinessUnitPicture, 400, 800);
            }

            if (viewModel.Picture != null)
            {
                optimizer.DeleteOldImage(viewModel.Picture);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;
            await this.businessUnitService.UpdateBusinessUnitAsync(model);

            return Ok(string.Format(WebConstants.BusinessUnitUpdated, viewModel.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModeratorToBusinessUnit(BusinessUnitViewModel viewModel)
        {
            await this.businessUnitService.AddModeratorToBusinessUnitsAsync(viewModel.ModeratorId, viewModel.Id);

            return Ok(string.Format(WebConstants.SuccessfullyAddedModeratorToBusinessUnit, viewModel.ModeratorId, viewModel.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveModerator(BusinessUnitViewModel model)
        {
            await this.businessUnitService.RemoveModeratorFromBusinessUnitsAsync(model.ModeratorId, model.Id);

            return Ok(string.Format(WebConstants.SuccessfullyRemovedModeratorFromBusinessUnit, model.ModeratorId, model.Id));
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