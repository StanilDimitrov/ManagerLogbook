using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            var model = viewModel.MapFrom();

            var review = await this.reviewService.CreateReviewAsync(model);

            if (review.OriginalDescription == viewModel.OriginalDescription)
            {
                return Ok(string.Format(WebConstants.ReviewCreated));
            }

            return BadRequest(string.Format(WebConstants.ReviewNotCreated));
        }
    }
}