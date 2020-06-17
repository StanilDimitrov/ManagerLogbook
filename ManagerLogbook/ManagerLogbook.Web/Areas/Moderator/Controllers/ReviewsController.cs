using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Web.Areas.Moderator.Models;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = "Moderator")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;
        private readonly IUserServiceWrapper _wrapper;

        public ReviewsController(IReviewService reviewService,
                                 IBusinessUnitService businessUnitService,
                                 IUserService userService,
                                 IUserServiceWrapper wrapper)
        {
            _reviewService = reviewService;
            _businessUnitService = businessUnitService;
            _userService = userService;
            _wrapper = wrapper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ReviewViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditReview));
            }

            var model = viewModel.MapFrom();
            var reviewDTO = await _reviewService.UpdateReviewAsync(model);

            if (reviewDTO.EditedDescription != model.EditedDescription)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditReview));
            }

            return Ok(string.Format(WebConstants.ReviewEdited));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _reviewService.MakeReviewInvisibleAsync(id);
            return Ok(string.Format(WebConstants.ReviewDeactivated));
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexReviewViewModel();

            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoByIdAsync(userId);

            if (user.BusinessUnitId.HasValue)
            {
                var businessUnit = await _businessUnitService.GetBusinessUnitDtoAsync(user.BusinessUnitId.Value);
                model.BusinessUnit = businessUnit.MapFrom();
            }
            else
            {
                return BadRequest(string.Format(WebConstants.BusinessUniNotAssigned));
            }

            var reviews = await _reviewService.GetReviewsByModeratorAsync(userId);

            model.Reviews = reviews.Select(x => x.MapFrom()).ToList();
            model.ModeratorId = userId;

            return View(model);
        }
    }
}