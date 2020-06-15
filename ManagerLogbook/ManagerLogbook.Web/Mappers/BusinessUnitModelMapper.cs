using ManagerLogbook.Services.Models;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class BusinessUnitModelMapper
    {
        public static BusinessUnitModel MapFrom(this BusinessUnitViewModel viewModel)
        {
            return new BusinessUnitModel()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Address = viewModel.Address,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
                Information = viewModel.Information,
                CategoryId = viewModel.CategoryId.Value,
                TownId = viewModel.TownId.Value,
                Picture = viewModel.Picture,
            };
        }
    }
}
