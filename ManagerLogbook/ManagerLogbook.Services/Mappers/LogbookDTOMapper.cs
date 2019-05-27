using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class LogbookDTOMapper
    {
        public static LogbookDTO ToDTO(this Logbook entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new LogbookDTO()
            {
                Id = entity.Id,
                Picture = entity.Picture,
                BusinessUnitName = entity.BusinessUnit.Name,
                Name = entity.Name
            };
        }
    }
}
