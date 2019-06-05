using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBusinessUnitService businessUnitService;

        public HomeController(IBusinessUnitService businessUnitService)
        {
            this.businessUnitService = businessUnitService ?? throw new ArgumentNullException(nameof(businessUnitService));
        }

        public async Task<IActionResult> Index()
        {
            var businessUnitsDTO = await this.businessUnitService.GetAllBusinessUnitsByCategoryIdAsync(1);
            //var businessUnitViewModel = businessUnitsDTO.Select(x => x.MapFrom()).ToList();

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
