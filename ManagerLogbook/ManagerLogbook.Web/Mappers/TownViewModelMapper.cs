using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
