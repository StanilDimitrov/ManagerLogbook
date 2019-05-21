﻿using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

            var businessUnit = new BusinessUnit() { BrandName = brandName, Address = address, PhoneNumber = phoneNumber, Email = email };

            this.context.BusinessUnits.Add(businessUnit);
            await this.context.SaveChangesAsync();

            return businessUnit;
        }

        public async Task<BusinessUnit> IsBusinessUnitExists(string brandName)
        {
            return await this.context.BusinessUnits.SingleOrDefaultAsync(bn => bn.BrandName == brandName);

        }

        public async Task<BusinessUnit> UpdateBusinessUnitAsync(BusinessUnit businessUnit, string brandName, string address, string phoneNumber, string email ,string picture)
        {
            if (brandName != null)
            {
                businessValidator.IsNameInRange(brandName);
            }

            businessUnit.BrandName = brandName;

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

            businessUnit.PhoneNumber = phoneNumber;

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
    }
}
