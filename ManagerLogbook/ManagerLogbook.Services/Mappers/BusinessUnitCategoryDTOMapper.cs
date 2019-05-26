using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class BusinessUnitCategoryDTOMapper
    {
        public static BusinessUnitCategoryDTO ToDTO(BusinessUnitCategory entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new BusinessUnitCategoryDTO()
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName
            };
        }
    }
}
