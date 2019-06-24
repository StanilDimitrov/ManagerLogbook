using System;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ManagerLogbook.Web.Mappers;

namespace ManagerLogbook.Web.Controllers
{
    public class BusinessUnitsController : Controller
    {
        private readonly IBusinessUnitService businessUnitService;
        private readonly IReviewService reviewService;

        public BusinessUnitsController(IBusinessUnitService businessUnitService,
                                       IReviewService reviewService)
        {
            this.businessUnitService = businessUnitService;
            this.reviewService = reviewService;            
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new IndexBusinessUnitViewModel();

            var businessUnit = await this.businessUnitService.GetBusinessUnitById(id);

            model.BusinessUnit = businessUnit.MapFrom();

            var reviewDTOs = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

            model.Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList();

            model.Logbooks = await this.businessUnitService.GetAllLogbooksForBusinessUnitAsync(businessUnit.Id);
                                         
            return View(model);
        }

        public async Task<IActionResult> GetReviewsList(int id)
        {
            var model = new IndexBusinessUnitViewModel();

            var reviewDTOs = await this.reviewService.GetAllReviewsByBusinessUnitIdAsync(id);

            model.Reviews = reviewDTOs.Select(x => x.MapFrom()).ToList();

            return PartialView("_ReviewsPartial", model);
        }

        public async Task<IActionResult> GiveLikeToBusinessUnit(int businessUnitId)
        {
            var model = new IndexBusinessUnitViewModel();

            await this.businessUnitService.GiveLikeBusinessUnitAsync(businessUnitId);

            var businessUnit = await this.businessUnitService.GetBusinessUnitById(businessUnitId);                       

            model.BusinessUnit = businessUnit.MapFrom();

            return View(model);
        }
    }
}