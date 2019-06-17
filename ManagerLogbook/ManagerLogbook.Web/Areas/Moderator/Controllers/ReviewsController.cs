using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Moderator.Models;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = "Moderator")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly IBusinessUnitService businessUnitService;
        private readonly IUserService userService;
        private readonly IUserServiceWrapper wrapper;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewsController));

        public ReviewsController(IReviewService reviewService,
                                 IBusinessUnitService businessUnitService,
                                 IUserService userService,
                                 IUserServiceWrapper wrapper)
        {
            this.reviewService = reviewService;
            this.businessUnitService = businessUnitService;
            this.userService = userService ;
            this.wrapper = wrapper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditReview));
            }

            try
            {
                var reviewDTO = await this.reviewService.GetReviewByIdAsync(model.Id);

                reviewDTO = await this.reviewService.UpdateReviewAsync(model.Id, model.EditedDescription);

                if (reviewDTO.EditedDescription != model.EditedDescription)
                {
                    return BadRequest(string.Format(WebConstants.UnableToEditReview));
                }

                return Ok(string.Format(WebConstants.ReviewEdited));
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

        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
               await this.reviewService.MakeInVisibleReviewAsync(id);                       
               return Ok(string.Format(WebConstants.ReviewDeactivated));
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new IndexReviewViewModel();

                var userId = this.wrapper.GetLoggedUserId(User);
                
                var user = await this.userService.GetUserByIdAsync(userId);

                var businessUnit = new BusinessUnitDTO();

                if (user.BusinessUnitId.HasValue)
                {
                    businessUnit = await this.businessUnitService.GetBusinessUnitById(user.BusinessUnitId.Value);
                }
                else
                {
                    return BadRequest(string.Format(WebConstants.BusinessUniNotAssigned));
                }

                var reviews = await this.reviewService.GetAllReviewsByModeratorIdAsync(userId);

                model.Reviews = reviews.Select(x => x.MapFrom()).ToList();
                model.BusinessUnit = businessUnit.MapFrom();
                model.ModeratorId = userId; 

                return View(model);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}