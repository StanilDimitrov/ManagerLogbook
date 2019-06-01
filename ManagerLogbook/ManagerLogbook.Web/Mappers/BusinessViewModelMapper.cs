using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class BusinessViewModelMapper
    {
        public static BusinessUnitViewModel MapFrom(this BusinessUnitDTO dto)
        {
            return new BusinessUnitViewModel()
            {
                Id = dto.Id,
                Name = dto.BrandName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Information = dto.Information,
                BusinessUnitCategoryName = dto.BusinessUnitCategoryName,
                BusinessUnitTownName = dto.BusinessUnitTownName
            };
        }

        
    }
}
