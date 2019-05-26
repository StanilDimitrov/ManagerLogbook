using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class BusinessUnitDTOMapper 
    {
        public  static BusinessUnitDTO ToDTO(BusinessUnit entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new BusinessUnitDTO()
            {
                Id = entity.Id,
                Address = entity.Address,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                PhoneNumber = entity.PhoneNumber,
                BrandName = entity.BrandName,
                Email = entity.Email,
                Picture = entity.Picture,
                BusinessUnitCategoryName = entity.BusinessUnitCategory.CategoryName
            };
        }
    }
}
