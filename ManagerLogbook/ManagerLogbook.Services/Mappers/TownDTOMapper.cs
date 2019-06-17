using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System.Linq;

namespace ManagerLogbook.Services.Mappers
{
    public static class TownDTOMapper
    {
        public static TownDTO ToDTO(this Town entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new TownDTO()
            {
                Id = entity.Id,
                Name = entity.Name
               //BusinessUnits = entity.BusinessUnits.Select(x => x.ToDTO()).ToList()
            };
        }
    }
}

