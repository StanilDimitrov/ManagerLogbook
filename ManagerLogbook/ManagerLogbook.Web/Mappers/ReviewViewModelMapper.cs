using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class ReviewViewModelMapper
    {
        public static ReviewViewModel ToDTO(this ReviewDTO dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new ReviewViewModel()
            {
                OriginalDescription = dto.OriginalDescription,
                EditedDescription = dto.EditedDescription,
                CreatedOn = dto.CreatedOn,
                isVisible = dto.isVisible,
                Rating = dto.Rating
            };
        }
    }
}
