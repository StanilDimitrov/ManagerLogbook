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

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BusinessUnitsController : Controller
    {
        private readonly IBusinessUnitService businessUnitService;
        private readonly IUserService userService;
        private readonly IImageOptimizer optimizer;

        public BusinessUnitsController(IBusinessUnitService businessUnitService,
                                      IUserService userService,
                                      IImageOptimizer optimizer)
        {
            this.businessUnitService = businessUnitService ?? throw new System.ArgumentNullException(nameof(businessUnitService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.optimizer = optimizer ?? throw new System.ArgumentNullException(nameof(optimizer));
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessUnitViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string imageName = null;

                if (model.Picture != null)
                {
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 268, 182);
                }

                var businessUnit = await this.businessUnitService.CreateBusinnesUnitAsync(model.Name, model.Address, model.PhoneNumber, model.Email, model.Information, model.CategoryId, model.TownId, model.Picture);

                if (businessUnit.BrandName == model.Name)
                {
                    return Ok(string.Format(WebConstants.BusinessUnitCreated));
                }

                return RedirectToAction("Error", "Home");
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var businessUnit = await this.businessUnitService.GetBusinessUnitById(id);
            var businessUnitViewModel = businessUnit.MapFrom();
            return View(businessUnitViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BusinessUnitViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToUpdateBusinessUnit));
            }

            try
            {
                var businessUnitDTO = await this.businessUnitService.GetBusinessUnitById(model.Id);

                string imageName = null;

                if (model.Picture != null)
                {
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 268, 182);
                }

                if (model.Picture != null)
                {
                    optimizer.DeleteOldImage(model.Picture);
                }

                businessUnitDTO = await this.businessUnitService.UpdateBusinessUnitAsync(model.Id, model.Name, model.Address, model.PhoneNumber, model.Information, model.Email, imageName);

                if (businessUnitDTO.BrandName != model.Name)
                {
                    return BadRequest(string.Format(WebConstants.UnableToUpdateBusinessUnit));
                }

                return Ok(string.Format(WebConstants.BusinessUnitUpdated));
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //public async Task<BusinessUnitDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId)
        [HttpGet]
        public async Task<IActionResult> AddModeratorToBusinessUnit(int businessUnitId)
        {
            var moderators = await this.userService.GetAllModeratorsAsync();

            if (moderators == null)
            {
                return BadRequest(string.Format(WebConstants.ModeratorNotExist));
            }

            var moderatorViewModel = moderators.Select(m => m.MapFrom()).ToList();

            ViewData["BusinessUnitId"] = businessUnitId;
            return View(moderatorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModeratorToBusinessUnit(string moderatorId, int businessUnitId)
        {
            try
            {
                var businessUnit = await this.businessUnitService.GetBusinessUnitById(businessUnitId);
                if (businessUnit == null)
                {
                    return BadRequest(string.Format(WebConstants.BusinessUniNotExist));
                }

                var moderator = await this.userService.GetUserByIdAsync(moderatorId);
                if (moderator == null)
                {
                    return BadRequest(string.Format(WebConstants.ModeratorNotExist));
                }

                await this.businessUnitService.AddModeratorToBusinessUnitsAsync(moderatorId, businessUnitId);

                StatusMessage = string.Format(WebConstants.SuccessfullyAddedModeratorToBusinessUnit, moderator.UserName, businessUnit.BrandName);

                return RedirectToAction("Details", "BusinessUnits", new { id = businessUnit.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("AddModeratorToBusinessUnit", "BusinessUJnit");
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
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
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
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}