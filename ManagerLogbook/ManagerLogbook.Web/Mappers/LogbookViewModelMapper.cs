using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class LogbookViewModelMapper
    {
        public static LogbookViewModel MapFrom(this LogbookDTO dto)
        {
            return new LogbookViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
                Picture = dto.Picture,
                BusinessUnitName = dto.BusinessUnitName
            };
        }
    }
}
