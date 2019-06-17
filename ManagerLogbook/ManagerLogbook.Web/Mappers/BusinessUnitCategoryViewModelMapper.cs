using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Mappers
{
    public static class BusinessUnitCategoryViewModelMapper
    {
        public static BusinessUnitCategoryViewModel MapFrom(this BusinessUnitCategoryDTO dto)
        {
            return new BusinessUnitCategoryViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }
    }
}
