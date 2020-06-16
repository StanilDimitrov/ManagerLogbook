using ManagerLogbook.Services.Models;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class LogbookModelMapper
    {
        public static LogbookModel MapFrom(this LogbookViewModel viewModel)
        {
            return new LogbookModel()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Picture = viewModel.Picture,
                BusinessUnitId = viewModel.BusinessUnitId
            };
        }
    }
}
