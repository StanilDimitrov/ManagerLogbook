using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class ReviewViewModelMapper
    {
        public static ReviewViewModel MapFrom(this ReviewDTO dto)
        {
            return new ReviewViewModel()
            {
                Id = dto.Id,
                OriginalDescription = dto.OriginalDescription,
                EditedDescription = dto.EditedDescription,
                CreatedOn = dto.CreatedOn,
                IsVisible = dto.isVisible,
                Rating = dto.Rating,
                BusinessUnitId = dto.BusinessUnitId
            };
        }
    }
}
