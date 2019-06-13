using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewsController));

        public ReviewsController(IReviewService reviewService,
                                 IBusinessUnitService businessUnitService,
                                 IUserService userService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.businessUnitService = businessUnitService ?? throw new ArgumentNullException(nameof(businessUnitService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    var model = new IndexReviewViewModel();

        //    var reviews = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

        //    model.Reviews = reviews.Select(x => x.MapFrom()).ToList();

        //    return View(model);
        //}

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

        public async Task<IActionResult> Index()
        {          
            var model = new IndexReviewViewModel();

            var userId = this.User.GetId();
            var user = await this.userService.GetUserByIdAsync(userId);

            //var businessUnit = await this.businessUnitService.GetBusinessUnitById(user);

            var reviews = await this.reviewService.GetAllReviewsByModeratorIdAsync(userId);

            model.Reviews = reviews.Select(x => x.MapFrom()).ToList();
            //model.BusinessUnit = 

            return View(model);            
        }
    }
}