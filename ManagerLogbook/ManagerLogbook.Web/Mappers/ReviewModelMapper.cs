using ManagerLogbook.Services.Models;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class ReviewModelMapper
    {
        public static ReviewModel MapFrom(this ReviewViewModel viewModel)
        {
            return new ReviewModel()
            {
                Id = viewModel.Id,
                OriginalDescription = viewModel.OriginalDescription,
                Rating = viewModel.Rating,
                BusinessUnitId = viewModel.BusinessUnitId
            };
        }
    }
}
