using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class BusinessViewModelMapper
    {
        public static BusinessUnitViewModel MapFrom(this BusinessUnitDTO dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new BusinessUnitViewModel()
            {
                Id = dto.Id,
                BrandName = dto.BrandName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                BusinessUnitCategoryName = dto.BusinessUnitCategoryName
            };
        }
    }
}
