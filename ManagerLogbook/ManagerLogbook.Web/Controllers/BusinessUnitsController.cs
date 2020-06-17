using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    public class BusinessUnitsController : Controller
    {
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IBusinessUnitEngine _businessUnitEngine;
        private readonly IReviewService _reviewService;

        public BusinessUnitsController(
            IBusinessUnitService businessUnitService,
            IBusinessUnitEngine  businessUnitEngine,
            IReviewService reviewService)
        {
            _businessUnitService = businessUnitService;
            _businessUnitEngine = businessUnitEngine;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var businessUnit = await _businessUnitService.GetBusinessUnitDtoAsync(id);
            var reviewDTOs = await _reviewService.GetReviewsByBusinessUnitAsync(id);

            var viewModel = new IndexBusinessUnitViewModel
            {
                BusinessUnit = businessUnit.MapFrom(),
                Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList(),
                Logbooks = await _businessUnitEngine.GetLogbooksForBusinessUnitAsync(businessUnit.Id)
            };
       
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsList(int id)
        {
            var reviewDTOs = await _reviewService.GetReviewsByBusinessUnitAsync(id);

            var viewModel = new IndexBusinessUnitViewModel
            {
                Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList()
            };

            return PartialView("_ReviewsPartial", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GiveLikeToBusinessUnit(int businessUnitId)
        {
            var businessUnit = await _businessUnitService.AddLikeToBusinessUnitAsync(businessUnitId);
            return Json(businessUnit.Likes);
        }
    }
}