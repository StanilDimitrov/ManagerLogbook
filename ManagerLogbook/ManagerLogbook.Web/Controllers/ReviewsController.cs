using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewsController));

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
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
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }               
    }
}