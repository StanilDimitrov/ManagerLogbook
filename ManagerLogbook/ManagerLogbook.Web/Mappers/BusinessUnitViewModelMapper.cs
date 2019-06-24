using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class BusinessUnitViewModelMapper
    {
        public static BusinessUnitViewModel MapFrom(this BusinessUnitDTO dto)
        {
            return new BusinessUnitViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Information = dto.Information,
                CategoryName = dto.CategoryName,
                TownName = dto.BusinessUnitTownName,
                Picture = dto.Picture,
                Likes = dto.Likes
            };
        }
    }
}
