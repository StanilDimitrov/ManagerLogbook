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
        private readonly IBusinessUnitService businessUnitService;
        private readonly IReviewService reviewService;

        public BusinessUnitsController(
            IBusinessUnitService businessUnitService,
            IReviewService reviewService)
        {
            this.businessUnitService = businessUnitService;
            this.reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var businessUnit = await this.businessUnitService.GetBusinessUnitById(id);
            var reviewDTOs = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

            var viewModel = new IndexBusinessUnitViewModel
            {
                BusinessUnit = businessUnit.MapFrom(),
                Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList(),
                Logbooks = await this.businessUnitService.GetAllLogbooksForBusinessUnitAsync(businessUnit.Id)
            };
       
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsList(int id)
        {
            var reviewDTOs = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

            var viewModel = new IndexBusinessUnitViewModel
            {
                Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList()
            };

            return PartialView("_ReviewsPartial", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GiveLikeToBusinessUnit(int businessUnitId)
        {
            await this.businessUnitService.GiveLikeBusinessUnitAsync(businessUnitId);

            var businessUnit = await this.businessUnitService.GetBusinessUnitById(businessUnitId);
            return Json(businessUnit.Likes);
        }
    }
}