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
using ManagerLogbook.Web.Mappers;

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LogbooksController : Controller
    {
        private readonly ILogbookService _logbookService;
        private readonly IUserService _userService;
        private readonly IImageOptimizer _optimizer;
        private static readonly ILog log = LogManager.GetLogger(typeof(LogbooksController));

        public LogbooksController(ILogbookService logbookService,
                                  IUserService userService,
                                  IImageOptimizer optimizer)
        {
            _logbookService = logbookService;
            _userService = userService;
            _optimizer = optimizer;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogbookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.LogbookPicture != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.LogbookPicture, 400, 800);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;

            var logbookDto = await _logbookService.CreateLogbookAsync(model);

            if (logbookDto.Name == viewModel.Name)
            {
                return Ok(string.Format(WebConstants.LogbookCreated, logbookDto.Name));
            }

            return BadRequest(string.Format(WebConstants.LogbookNotCreated, logbookDto.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LogbookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToUpdateLogbook, viewModel.Name));
            }

            string imageName = null;

            if (viewModel.LogbookPicture != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.LogbookPicture, 400, 800);
            }

            if (viewModel.Picture != null)
            {
                _optimizer.DeleteOldImage(viewModel.Picture);
            }

            var model = viewModel.MapFrom();
            model.Picture = imageName;
            var logbookDTO = await _logbookService.UpdateLogbookAsync(model);

            if (logbookDTO.Name != viewModel.Name)
            {
                return BadRequest(string.Format(WebConstants.UnableToUpdateLogbook, viewModel.Name));
            }

            return Ok(string.Format(WebConstants.LogbookUpdated, viewModel.Name));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManagerToLogbook(LogbookViewModel model)
        {
            await _logbookService.AddManagerToLogbookAsync(model.ManagerId, model.Id);
            return Ok(WebConstants.SuccessfullyAddedManagerToLogbook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveManagerFromLogbook(LogbookViewModel model)
        {

            await _logbookService.RemoveManagerFromLogbookAsync(model.ManagerId, model.Id);

            return Ok(WebConstants.SuccessfullyRemovedManagerFromLogbook);

        }

        public async Task<IActionResult> GetAllManagersNotPresent(int id)
        {
            var managers = await _userService.GetAllManagersNotPresentInLogbookAsync(id);

            if (managers == null)
            {
                return BadRequest(string.Format(WebConstants.ManagerNotExist));
            }

            return Json(managers);
        }

        public async Task<IActionResult> GetAllManagersPresent(int id)
        {
            var managers = await _userService.GetAllManagersPresentInLogbookAsync(id);

            if (managers == null)
            {
                return BadRequest(string.Format(WebConstants.ManagerNotExist));
            }

            return Json(managers);
        }
    }
}