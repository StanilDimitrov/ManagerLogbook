using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Please enter valid data");
            }

            try
            {
                var review = await this.reviewService.CreateReviewAsync(model.OriginalDescription, model.BusinessUnitId,model.Rating);

                if (review.OriginalDescription == model.OriginalDescription)
                {
                    return Ok(string.Format(WebConstants.ReviewCreated));
                }

                return BadRequest(string.Format(WebConstants.ReviewNotCreated));
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
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