using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Services.CustomExeptions;

namespace ManagerLogbook.Services
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly ManagerLogbookContext context;
        private readonly IBusinessValidator businessValidator;

        public BusinessUnitService(ManagerLogbookContext context,
                                   IBusinessValidator businessValidator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.businessValidator = businessValidator ?? throw new ArgumentNullException(nameof(businessValidator));
        }

        public async Task<BusinessUnitDTO> CreateBusinnesUnitAsync(string brandName, string address, string phoneNumber, string email, string information, int businessUnitCategoryId, int townId, string picture)
        {
            var checkBrandNameIfExists = await this.context.BusinessUnits
                                           .FirstOrDefaultAsync(n => n.Name == brandName);

            if (checkBrandNameIfExists != null)
            {
                throw new AlreadyExistsException(ServicesConstants.BusinessUnitNameAlreadyExists);
            }

            businessValidator.IsNameInRange(brandName);
            businessValidator.IsAddressInRange(address);
            businessValidator.IsEmailValid(email);
            businessValidator.IsPhoneNumberValid(phoneNumber);
            businessValidator.IsDescriptionInRange(information);

            var businessUnit = new BusinessUnit() { Name = brandName, Address = address, PhoneNumber = phoneNumber, Email = email, Information = information, BusinessUnitCategoryId = businessUnitCategoryId, TownId = townId, Picture = picture };

            await this.context.BusinessUnits.AddAsync(businessUnit);
            await this.context.SaveChangesAsync();

            var result = await this.context.BusinessUnits
                                           .Include(buc => buc.BusinessUnitCategory)
                                           .Include(t => t.Town)
                                           .FirstOrDefaultAsync(x => x.Id == businessUnit.Id);

            return result.ToDTO();
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

        public async Task<BusinessUnitDTO> UpdateBusinessUnitAsync(int businessUnitId, string brandName, string address, string phoneNumber, string information, string email, int businessUnitCategoryId, int townId, string picture)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            if (brandName != null)
            {
                businessValidator.IsNameInRange(brandName);
            }

            var checkBrandNameIfExists = await this.context.BusinessUnits
                                           .FirstOrDefaultAsync(n => n.Name == brandName);

            if (checkBrandNameIfExists != null && businessUnit.Name != brandName)
            {
                throw new AlreadyExistsException(ServicesConstants.BusinessUnitNameAlreadyExists);
            }

            businessUnit.Name = brandName;

            if (address != null)
            {
                businessValidator.IsAddressInRange(address);
            }

            businessUnit.Address = address;

            if (phoneNumber != null)
            {
                businessValidator.IsPhoneNumberValid(phoneNumber);
            }

            businessUnit.PhoneNumber = phoneNumber;

            if (email != null)
            {
                businessValidator.IsEmailValid(email);
            }

            businessUnit.Email = email;

            if (information != null)
            {
                businessValidator.IsDescriptionInRange(information);
            }

            businessUnit.Information = information;



            if (picture != null)
            {
                businessUnit.Picture = picture;
            }

            await this.context.SaveChangesAsync();

            var result = await this.context.BusinessUnits
                                           .Include(buc => buc.BusinessUnitCategory)
                                           .Include(t => t.Town)
                                           .FirstOrDefaultAsync(x => x.Id == businessUnit.Id);

            return result.ToDTO();
        }

        public async Task<IReadOnlyCollection<LogbookDTO>> GetAllLogbooksForBusinessUnitAsync(int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

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

        public async Task<BusinessUnitCategoryDTO> CreateBusinessUnitCategoryAsync(string businessUnitCategoryName)
        {
            businessValidator.IsNameInRange(businessUnitCategoryName);

            var checkBusinessUnitCategoryNameIfExists = await this.context.BusinessUnitCategories
                                           .FirstOrDefaultAsync(n => n.Name == businessUnitCategoryName);

            if (checkBusinessUnitCategoryNameIfExists != null)
            {
                throw new AlreadyExistsException(ServicesConstants.BusinessUnitCategoryNameAlreadyExists);
            }

            var businessUnitCategory = new BusinessUnitCategory() { Name = businessUnitCategoryName };

            await this.context.SaveChangesAsync();

            return businessUnitCategory.ToDTO();
        }

        public async Task<BusinessUnitCategoryDTO> UpdateBusinessUnitCategoryAsync(int businessUnitCategoryId, string newBusinessUnitCategoryName)
        {
            businessValidator.IsNameInRange(newBusinessUnitCategoryName);

            var checkBusinessUnitCategoryNameIfExists = await this.context.BusinessUnitCategories
                                           .FirstOrDefaultAsync(n => n.Name == newBusinessUnitCategoryName);

            if (checkBusinessUnitCategoryNameIfExists != null)
            {
                throw new AlreadyExistsException(ServicesConstants.BusinessUnitCategoryNameAlreadyExists);
            }

            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);

            if (businessUnitCategory == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitCategoryNotFound);
            }

            businessUnitCategory.Name = newBusinessUnitCategoryName;

            await this.context.SaveChangesAsync();

            return businessUnitCategory.ToDTO();
        }

        public async Task<BusinessUnitDTO> AddBusinessUnitCategoryToBusinessUnitAsync(int businessUnitCategoryId, int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);

            if (businessUnitCategory == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitCategoryNotFound);
            }

            businessUnit.BusinessUnitCategoryId = businessUnitCategoryId;

            await this.context.SaveChangesAsync();

            var result = await this.context.BusinessUnits
                                           .Include(buc => buc.BusinessUnitCategory)
                                           .Include(t => t.Town)
                                           .FirstOrDefaultAsync(x => x.Id == businessUnit.Id);

            return result.ToDTO();
        }

        public async Task<BusinessUnitCategoryDTO> GetBusinessUnitCategoryByIdAsync(int businessUnitCategoryId)
        {
            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);

            if (businessUnitCategory == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitCategoryNotFound);
            }

            return businessUnitCategory.ToDTO();
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

        public async Task<BusinessUnitDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            var moderatorUser = await this.context.Users.FindAsync(moderatorId);

            if (moderatorUser == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            moderatorUser.BusinessUnitId = businessUnitId;
            await this.context.SaveChangesAsync();
            businessUnit = await this.context.BusinessUnits
                         .Include(bc => bc.BusinessUnitCategory)
                         .Include(t => t.Town)
                         .FirstOrDefaultAsync(x => x.Id == businessUnitId);

            return businessUnit.ToDTO();
        }

        //public async Task RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId)
        public async Task<BusinessUnitDTO> RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            var moderatorUser = await this.context.Users.FindAsync(moderatorId);

            if (moderatorUser == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            moderatorUser.BusinessUnitId = null;
            await this.context.SaveChangesAsync();

            businessUnit = await this.context.BusinessUnits
                         .Include(bc => bc.BusinessUnitCategory)
                         .Include(t => t.Town)
                         .FirstOrDefaultAsync(x => x.Id == businessUnitId);

            return businessUnit.ToDTO();
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
    }
}
