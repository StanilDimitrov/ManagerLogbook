using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewsController));

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            var review = await this.reviewService.CreateReviewAsync(model.OriginalDescription, model.BusinessUnitId, model.Rating);

            if (review.OriginalDescription == model.OriginalDescription)
            {
                return Ok(string.Format(WebConstants.ReviewCreated));
            }

            return BadRequest(string.Format(WebConstants.ReviewNotCreated));
        }
    }
}