using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class ReviewDTOMapper
    {
        public static ReviewDTO ToDTO(Review entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new ReviewDTO()
            {
               Id = entity.Id,
               BusinessUnitName = entity.BusinessUnit.BrandName,
               OriginalDescription = entity.OriginalDescription,
               EditedDescription = entity.EditedDescription,
               CreatedOn = entity.CreatedOn,
               Rating  = entity.Rating,
               isVisible = entity.isVisible
            };
        }
    }
}
