using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<BusinessUnit> CreateBusinnesUnitAsync(string brandName, string address, string phoneNumber, string email)
        {
            businessValidator.IsNameInRange(brandName);
            businessValidator.IsAddressInRange(address);
            businessValidator.IsEmailValid(email);
            businessValidator.IsPhoneNumberValid(phoneNumber);

            var businessUnit = new BusinessUnit() { Name = brandName, Address = address, PhoneNumber = phoneNumber, Email = email };

            this.context.BusinessUnits.Add(businessUnit);
            await this.context.SaveChangesAsync();

            return businessUnit;
        }

        public async Task<BusinessUnit> GetBusinessUnitById(int businessUnitId)
        {
            return await this.context.BusinessUnits.FindAsync(businessUnitId);

        }

        public async Task<BusinessUnit> UpdateBusinessUnitAsync(BusinessUnit businessUnit, string brandName, string address, string phoneNumber, string email, string picture)
        {
            if (brandName != null)
            {
                businessValidator.IsNameInRange(brandName);
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

            if (picture != null)
            {
                businessUnit.Picture = picture;
            }

            await this.context.SaveChangesAsync();

            return businessUnit;
        }

        public async Task<BusinessUnit> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId)
        {
            var logbook = await this.context.Logbooks.FindAsync(logbookId);
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            logbook.BusinessUnitId = businessUnitId;

            await this.context.SaveChangesAsync();

            return businessUnit;
        }

        public async Task<IReadOnlyCollection<Logbook>> GetAllLogbooksForBusinessUnitAsync(int businessUnitId)
        {
            var logbooks = await this.context.Logbooks
                         .Where(bu => bu.BusinessUnitId == businessUnitId)
                         .ToListAsync();

            return logbooks;
        }

        public async Task<BusinessUnitCategory> CreateBusinessUnitCategoryAsync(string businessUnitCategoryName)
        {
            businessValidator.IsNameInRange(businessUnitCategoryName);

            var businessUnitCategory = new BusinessUnitCategory() { Name = businessUnitCategoryName };

            await this.context.SaveChangesAsync();

            return businessUnitCategory;
        }

        public async Task<BusinessUnitCategory> UpdateBusinessUnitCategoryAsync(int businessUnitCategoryId, string newBusinessUnitCategoryName)
        {
            businessValidator.IsNameInRange(newBusinessUnitCategoryName);

            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);

            businessUnitCategory.Name = newBusinessUnitCategoryName;

            await this.context.SaveChangesAsync();

            return businessUnitCategory;
        }

        public async Task<BusinessUnit> AddBusinessUnitCategoryToBusinessUnitAsync(int businessUnitCategoryId, int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            businessUnit.BusinessUnitCategoryId = businessUnitCategoryId;

            await this.context.SaveChangesAsync();

            return businessUnit;
        }

        public async Task<BusinessUnitCategory> GetBusinessUnitCategoryByIdAsync(int businessUnitCategoryId)
        {
            var businessUnitCategory = await this.context.BusinessUnitCategories.FindAsync(businessUnitCategoryId);                      

            await this.context.SaveChangesAsync();

            return businessUnitCategory;
        }

        public async Task<IReadOnlyCollection<BusinessUnit>> GetAllBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId)
        {
            var businessUnits = await this.context.BusinessUnits
                         .Where(bc => bc.BusinessUnitCategoryId == businessUnitCategoryId)
                         .ToListAsync();

            return businessUnits;
        }
    }
}
