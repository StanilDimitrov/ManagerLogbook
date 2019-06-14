using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Web.Areas.Moderator.Models;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Areas.Moderator.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewsController));

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new IndexReviewViewModel();

            var reviews = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

            model.Reviews = reviews.Select(x => x.MapFrom()).ToList();

            return View(model);
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

        public IActionResult Index()
        {
            return View();
        }
    }
}