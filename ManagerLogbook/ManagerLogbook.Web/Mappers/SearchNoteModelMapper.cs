using ManagerLogbook.Services.Models;
using ManagerLogbook.Web.Areas.Manager.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class SearchNoteModelMapper
    {
        public static SearchNoteModel MapFrom(this SearchViewModel viewModel)
        {
            return new SearchNoteModel()
            {
                StartDate = viewModel.StartDate,
                EndDate = viewModel.StartDate,
                CategoryId = viewModel.CategoryId,
                SearchCriteria = viewModel.SearchCriteria,
                DaysBefore = viewModel.DaysBefore,
                CurrPage = viewModel.CurrPage
            };
        }
    }
}
