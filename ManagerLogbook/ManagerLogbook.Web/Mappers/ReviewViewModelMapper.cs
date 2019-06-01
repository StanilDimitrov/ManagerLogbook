using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class ReviewViewModelMapper
    {
        public static ReviewViewModel ToDTO(this ReviewDTO dto)
        {
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
