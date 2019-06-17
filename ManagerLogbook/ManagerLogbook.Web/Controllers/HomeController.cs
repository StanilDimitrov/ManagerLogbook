using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            this.businessUnitService = businessUnitService ?? throw new ArgumentNullException(nameof(businessUnitService));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IActionResult> Index(HomeViewModel model)
        {
           
            var businessUnitsDTO = await this.businessUnitService.GetAllBusinessUnitsAsync();
            var categoriesDTO = await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();
            var townsDTO = await this.businessUnitService.GetAllTownsAsync();

            //var businessUnits = (await CacheBusinessUnits()).Select(x => x.MapFrom()).ToList();
            model.Towns = (await CacheTowns()).Select(x => x.MapFrom()).ToList();
            model.Categories = (await CacheCategories()).Select(x => x.MapFrom()).ToList();
            var businessCategories = await this.businessUnitService.GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();
            model.Footer = new FooterViewModel()
            {
                BusinessUnitsCategories = businessCategories
            };
            model.SearchModelBusiness = new BusinessUnitSearchViewModel()
            {
                BusinessUnits = businessUnitsDTO.Select(x => x.MapFrom()).ToList()
        };
            return View(model);
        }

        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 86400)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public async Task<IActionResult> Search(HomeViewModel model)
        {
            var businessUnitsDTO = await this.businessUnitService
                                             .SearchBusinessUnitsAsync(model.SearchCriteria, model.CategoryId,
                                             model.TownId);

            var townsDTO = await this.businessUnitService.GetAllTownsAsync();
            var categoriesDTO = await this.businessUnitService.GetAllBusinessUnitsCategoriesAsync();

            model.BusinessUnits = businessUnitsDTO.Select(x => x.MapFrom()).ToList();
            model.Towns = townsDTO.Select(x => x.MapFrom()).ToList();
            model.Categories = categoriesDTO.Select(x => x.MapFrom()).ToList();

            var businessCategories = await this.businessUnitService.GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();
            var footer = new FooterViewModel()
            {
                BusinessUnitsCategories = businessCategories
            };

            var searchModel = new BusinessUnitSearchViewModel();

            searchModel.BusinessUnits = businessUnitsDTO.Select(x => x.MapFrom()).ToList();

            return PartialView("_BusinessUnitsPartial", searchModel);

        }


        private async Task<BusinessUnitViewModel> CreateDropdownNoteCategories(BusinessUnitViewModel model)
        {
            var cashedCategories = await CacheCategories();

            model.Categories = cashedCategories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<BusinessUnitViewModel> EditDropdownNoteCategories(BusinessUnitViewModel model)
        {

            var cashedCategories = await CacheCategories();

            List<SelectListItem> selectCategories = new List<SelectListItem>();

            foreach (var category in cashedCategories)
            {
                if (category.Name == model.CategoryName)
                {
                    selectCategories.Add(new SelectListItem(category.Name, category.Id.ToString(), true));
                }
                else
                {
                    selectCategories.Add(new SelectListItem(category.Name, category.Id.ToString()));
                }
            }

            model.Categories = selectCategories;

            return model;
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


        private async Task<BusinessUnitViewModel> CreateDropdownTowns(BusinessUnitViewModel model)
        {
            var cashedTowns = await CacheTowns();

            model.Categories = cashedTowns.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<BusinessUnitViewModel> EditDropdownTowns(BusinessUnitViewModel model)
        {

            var cashedTowns = await CacheTowns();

            List<SelectListItem> selectTowns = new List<SelectListItem>();

            foreach (var category in cashedTowns)
            {
                if (category.Name == model.CategoryName)
                {
                    selectTowns.Add(new SelectListItem(category.Name, category.Id.ToString(), true));
                }
                else
                {
                    selectTowns.Add(new SelectListItem(category.Name, category.Id.ToString()));
                }
            }

            model.Categories = selectTowns;

            return model;
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
