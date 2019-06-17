using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System.Linq;

namespace ManagerLogbook.Services.Mappers
{
    public static class BusinessUnitDTOMapper
    {
        public static BusinessUnitDTO ToDTO(this BusinessUnit entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new BusinessUnitDTO()
            {
                Id = entity.Id,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                Name = entity.Name,
                Email = entity.Email,
                Picture = entity.Picture,
                Information = entity.Information,
                CategoryName = entity.BusinessUnitCategory.Name,
                BusinessUnitTownName = entity.Town.Name
            };
        }
    }
}
