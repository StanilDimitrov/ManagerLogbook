using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Models.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log =
        LogManager.GetLogger(typeof(HomeController));

        private readonly IBusinessUnitService businessUnitService;
        private readonly IMemoryCache cache;

        public HomeController(IBusinessUnitService businessUnitService, IMemoryCache cache)
        {
            this.businessUnitService = businessUnitService;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var businessUnitsDTO = await this.businessUnitService.GetAllBusinessUnitsAsync();
            var categoriesDTO = await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();
            var townsDTO = await this.businessUnitService.GetAllTownsAsync();
            var businessCategories = await this.businessUnitService.GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();

            var viewModel = new HomeViewModel
            {
                Towns = (await CacheTowns()).Select(x => x.MapFrom()).ToList(),
                Categories = (await CacheCategories()).Select(x => x.MapFrom()).ToList(),
                Footer = new FooterViewModel
                {
                    BusinessUnitsCategories = businessCategories
                },
                SearchModelBusiness = new BusinessUnitSearchViewModel
                {
                    BusinessUnits = businessUnitsDTO.Select(x => x.MapFrom()).ToList()
                }
            };

            return View(viewModel);
        }

        public ActionResult Redirect()
        {
            if (User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Notes", new { area = "Manager" });
            }
            if (User.IsInRole("Moderator"))
            {
                return RedirectToAction("Index", "Reviews", new { area = "Moderator" });
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 86400)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchBusinessUnitBindngModel bindngModel)
        {
            var businessUnitsDTO = await this.businessUnitService.SearchBusinessUnitsAsync(
                bindngModel.SearchCriteria,
                bindngModel.CategoryId,
                bindngModel.TownId);

            var townsDTO = await this.businessUnitService.GetAllTownsAsync();
            var categoriesDTO = await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();
            var businessCategories = await this.businessUnitService.GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();

            var viewModel = new BusinessUnitSearchViewModel
            {
                BusinessUnits = businessUnitsDTO.Select(x => x.MapFrom()).ToList()
            };

            return PartialView("_BusinessUnitsPartial", viewModel);
        }

        private async Task<IReadOnlyCollection<BusinessUnitCategoryDTO>> CacheCategories()
        {
            var cashedCategories = await cache.GetOrCreateAsync<IReadOnlyCollection<BusinessUnitCategoryDTO>>("Categories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();
            });

            return cashedCategories;
        }

        private async Task<IReadOnlyCollection<TownDTO>> CacheTowns()
        {
            var cashedTowns = await cache.GetOrCreateAsync<IReadOnlyCollection<TownDTO>>("Towns", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.businessUnitService.GetAllTownsAsync();
            });

            return cashedTowns;
        }

        private async Task<IReadOnlyCollection<BusinessUnitDTO>> CacheBusinessUnits()
        {
            var cashedBusinessUnits = await cache.GetOrCreateAsync<IReadOnlyCollection<BusinessUnitDTO>>("BusinessUnits", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.businessUnitService.GetAllBusinessUnitsAsync();
            });

            return cashedBusinessUnits;
        }
    }
}
