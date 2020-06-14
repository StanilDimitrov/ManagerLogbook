using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly ManagerLogbookContext context;
        private readonly IUserService userService;

        public BusinessUnitService(ManagerLogbookContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task CreateBusinnesUnitAsync(BusinessUnitModel model)
        {
            await CheckIfBrandNameExist(model.Name);

            var businessUnit = new BusinessUnit
            {
                Name = model.Name,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Information = model.Information,
                BusinessUnitCategoryId = model.CategoryId,
                TownId = model.TownId,
                Picture = model.Picture
            };

            this.context.BusinessUnits.Add(businessUnit);
            await this.context.SaveChangesAsync();
        }

        public async Task<BusinessUnitDTO> GetBusinessUnitById(int businessUnitId)
        {
            var result = await this.context.BusinessUnits
                                           .Include(buc => buc.BusinessUnitCategory)
                                           .Include(t => t.Town)
                                           .FirstOrDefaultAsync(x => x.Id == businessUnitId);
            if (result == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            return result.ToDTO();
        }

        public async Task UpdateBusinessUnitAsync(BusinessUnitModel model)
        {
            var businessUnit = await GetBusinessUnitAsync(model.Id);
            await CheckIfBrandNameExist(model.Name);
            SetBusinessUnitProperties(model, businessUnit);
            await this.context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<LogbookDTO>> GetAllLogbooksForBusinessUnitAsync(int businessUnitId)
        {
            var businessUnit = await GetBusinessUnitAsync(businessUnitId);

            var logbooksDTO = await this.context.Logbooks
                         .Include(n => n.Notes)
                         .ThenInclude(u => u.User)
                         .Include(bu => bu.BusinessUnit)
                         .ThenInclude(t => t.Town)
                         .Where(bu => bu.BusinessUnitId == businessUnitId)
                         .Select(x => x.ToDTO())
                         .ToListAsync();

            return logbooksDTO;
        }

        public async Task<IReadOnlyCollection<BusinessUnitDTO>> GetAllBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId)
        {
            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);

            if (businessUnitCategory == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitCategoryNotFound);
            }
            var businessUnits = await this.context.BusinessUnits
                         .Include(buc => buc.BusinessUnitCategory)
                         .Include(t => t.Town)
                         .Where(bc => bc.BusinessUnitCategoryId == businessUnitCategoryId)
                         .Select(bu => bu.ToDTO())
                         .ToListAsync();

            return businessUnits;
        }

        public async Task<IReadOnlyCollection<BusinessUnitDTO>> GetAllBusinessUnitsAsync()
        {
            var businessUnitsDTO = await this.context.BusinessUnits
                         .Include(buc => buc.BusinessUnitCategory)
                         .Include(t => t.Town)
                         .OrderByDescending(id => id.Id)
                         .Select(x => x.ToDTO())
                         .ToListAsync();

            return businessUnitsDTO;
        }

        public async Task<IReadOnlyCollection<TownDTO>> GetAllTownsAsync()
        {
            var townsDTO = await this.context.Towns
                                          .Include(x => x.BusinessUnits)
                                          .OrderBy(n => n.Name)
                                          .Select(x => x.ToDTO())
                                          .ToListAsync();
            return townsDTO;
        }

        public async Task AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            await GetBusinessUnitAsync(businessUnitId);

            var moderatorUser = await this.userService.GetUserAsync(moderatorId);
            moderatorUser.BusinessUnitId = businessUnitId;
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            await GetBusinessUnitAsync(businessUnitId);

            var moderatorUser = await this.userService.GetUserAsync(moderatorId);
            moderatorUser.BusinessUnitId = null;
            await this.context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<BusinessUnitDTO>> SearchBusinessUnitsAsync(string searchCriteria, int? businessUnitCategoryId, int? townId)
        {
            IQueryable<BusinessUnit> searchCollection;
            if (searchCriteria != null)
            {
                searchCollection = this.context.BusinessUnits
                    .Include(x => x.BusinessUnitCategory)
                    .Include(x => x.Town)
                    .Where(n => n.Name.ToLower().Contains(searchCriteria.ToLower()));
            }
            else
            {
                searchCollection = this.context.BusinessUnits
                    .Include(x => x.BusinessUnitCategory)
                    .Include(x => x.Town);
            }

            if (businessUnitCategoryId != null)
            {
                searchCollection = searchCollection.Where(buc => buc.BusinessUnitCategoryId == businessUnitCategoryId);
            }

            if (townId != null)
            {
                searchCollection = searchCollection.Where(t => t.TownId == townId);
            }

            var searchResult = await searchCollection.Select(x => x.ToDTO()).ToListAsync();
            return searchResult;
        }

        public async Task<IReadOnlyCollection<BusinessUnitCategoryDTO>> GetAllBusinessUnitsCategoriesAsync()
        {
            var businessUnitsCategoriesDTO = await this.context.BusinessUnitCategories
                         .OrderBy(n => n.Name)
                         .Select(x => x.ToDTO())
                         .ToListAsync();

            return businessUnitsCategoriesDTO;
        }

        public async Task<IReadOnlyDictionary<string, int>> GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync()
        {
            var categoriesCountUnits = await this.context.BusinessUnits
                         .Include(bu => bu.BusinessUnitCategory)
                         .GroupBy(c => c.BusinessUnitCategory.Name)
                         .Select(group => new
                         {
                             categoryName = group.Key,
                             count = group.Count()
                         })
                         .OrderBy(c => c.categoryName)
                         .ToDictionaryAsync(x => x.categoryName, x => x.count);

            return categoriesCountUnits;
        }

        public async Task<BusinessUnitDTO> GiveLikeBusinessUnitAsync(int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            businessUnit.Likes++;

            await this.context.SaveChangesAsync();

            var result = await this.context.BusinessUnits
                                           .Include(buc => buc.BusinessUnitCategory)
                                           .Include(t => t.Town)
                                           .FirstOrDefaultAsync(x => x.Id == businessUnit.Id);
            return result.ToDTO();
        }

        private async Task<BusinessUnit> GetBusinessUnitAsync(int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.SingleOrDefaultAsync(bu => bu.Id == businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException($"Business unit with id: {businessUnitId} does not exist.");
            }

            return businessUnit;
        }

        private async Task CheckIfBrandNameExist(string brandName)
        {
            var businessUnit = await this.context.BusinessUnits.SingleOrDefaultAsync(bu => bu.Name == brandName);

            if (businessUnit != null)
            {
                throw new AlreadyExistsException($"Business unit with name: {brandName} already exists.");
            }

        }

        private void SetBusinessUnitProperties(BusinessUnitModel model, BusinessUnit entity)
        {
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Email = model.Email;
            entity.Information = model.Information;
           
            if (model.Picture != null)
            {
                entity.Picture = model.Picture;
            }

            if (model.TownId != 0)
            {
                entity.TownId = model.TownId;
            }

            if (model.CategoryId != 0)
            {
                entity.BusinessUnitCategoryId = model.CategoryId;
            }
        }
        
    }
}
