using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class TownViewModelMapper
    {
        public static TownViewModel MapFrom(this TownDTO dto)
        {
            return new TownViewModel()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
