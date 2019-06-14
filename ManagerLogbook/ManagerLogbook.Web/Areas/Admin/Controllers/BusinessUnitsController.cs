using System;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            this.businessUnitService = businessUnitService ?? throw new System.ArgumentNullException(nameof(businessUnitService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.optimizer = optimizer ?? throw new System.ArgumentNullException(nameof(optimizer));
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

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
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 268, 182);
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

            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Update(int id)
        //{
        //    var businessUnit = await this.businessUnitService.GetBusinessUnitById(id);
        //    var businessUnitViewModel = businessUnit.MapFrom();
        //    return View(businessUnitViewModel);
        //}


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

                if (model.Picture != null)
                {
                    imageName = optimizer.OptimizeImage(model.BusinessUnitPicture, 268, 182);
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

        //[HttpGet]
        //public async Task<IActionResult> AddModeratorToBusinessUnit(int businessUnitId)
        //{
        //    var moderators = await this.userService.GetAllModeratorsNotPresentInBusinessUnitAsync(businessUnitId);

        //    if (moderators == null)
        //    {
        //        return BadRequest(string.Format(WebConstants.ModeratorNotExist));
        //    }

        //    var moderatorViewModel = moderators.Select(m => m.MapFrom()).ToList();

        //    return View(moderatorViewModel);
        //}

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

        //[HttpGet]
        //public async Task<IActionResult> RemoveModeratorFromBusinessUnit(int businessUnitId)
        //{
        //    var moderators = await this.userService.GetAllModeratorsPresentInBusinessUnitAsync(businessUnitId);

        //    if (moderators == null)
        //    {
        //        return BadRequest(string.Format(WebConstants.ModeratorNotExist));
        //    }

        //    var moderatorViewModel = moderators.Select(m => m.MapFrom()).ToList();

        //    return View(moderatorViewModel);
        //}

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

        public IActionResult Index()
        {
            return View();
        }
    }
}