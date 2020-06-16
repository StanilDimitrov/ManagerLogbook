using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BusinessUnitsController : Controller
    {
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;
        private readonly IImageOptimizer _optimizer;
        private static readonly ILog log = LogManager.GetLogger(typeof(BusinessUnitsController));

        public BusinessUnitsController(IBusinessUnitService businessUnitService,
                                      IUserService userService,
                                      IImageOptimizer optimizer)
        {
            _businessUnitService = businessUnitService;
            _userService = userService;
            _optimizer = optimizer;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessUnitViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.BusinessUnitPicture != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.BusinessUnitPicture, 400, 800);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;
            await _businessUnitService.CreateBusinnesUnitAsync(model);

            return Ok(string.Format(WebConstants.BusinessUnitCreated, viewModel.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BusinessUnitViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.BusinessUnitPicture != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.BusinessUnitPicture, 400, 800);
            }

            if (viewModel.Picture != null)
            {
                _optimizer.DeleteOldImage(viewModel.Picture);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;

            var businessUnitDto = await _businessUnitService.UpdateBusinessUnitAsync(model);
            return Ok(string.Format(WebConstants.BusinessUnitUpdated, businessUnitDto.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModeratorToBusinessUnit(BusinessUnitViewModel viewModel)
        {
            var userDto = await _businessUnitService.AddModeratorToBusinessUnitsAsync(viewModel.ModeratorId, viewModel.Id);
            return Ok(string.Format(WebConstants.SuccessfullyAddedModeratorToBusinessUnit, userDto.UserName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveModerator(BusinessUnitViewModel model)
        {
            var userDto = await _businessUnitService.RemoveModeratorFromBusinessUnitsAsync(model.ModeratorId, model.Id);
            return Ok(string.Format(WebConstants.SuccessfullyRemovedModeratorFromBusinessUnit, userDto.UserName));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinessUnitCategories()
        {

            var categories = await _businessUnitService.GetBusinessUnitsCategoriesAsync();
            return Json(categories);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinessUnits()
        {
            var businessUnits = await _businessUnitService.GetBusinessUnitsAsync();
            return Json(businessUnits);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllTowns()
        {
            var towns = await _businessUnitService.GetAllTownsAsync();
            return Json(towns);
        }

        public async Task<IActionResult> GetAllModeratorsNotPresent(int id)
        {
            var moderators = await _userService.GetAllModeratorsNotPresentInBusinessUnitAsync(id);

            if (moderators == null)
            {
                return BadRequest(string.Format(WebConstants.ModeratorNotExist));
            }

            return Json(moderators);
        }

        public async Task<IActionResult> GetAllModeratorsPresent(int id)
        {
            var moderators = await _userService.GetAllModeratorsPresentInBusinessUnitAsync(id);

            if (moderators == null)
            {
                return BadRequest(string.Format(WebConstants.ModeratorNotExist));
            }

            return Json(moderators);
        }
    }
}