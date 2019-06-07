using System;
using System.Collections.Generic;
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

        public BusinessUnitsController(IBusinessUnitService businessUnitService)
        {
            this.businessUnitService = businessUnitService ?? throw new ArgumentNullException(nameof(businessUnitService));

        }

        public async Task<IActionResult> Details(int id)
        {
            var businessUnit = await this.businessUnitService.GetBusinessUnitById(id);

            var model = businessUnit.MapFrom();

            return View(model);
        }
    }
}