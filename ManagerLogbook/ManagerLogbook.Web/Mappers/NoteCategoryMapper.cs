using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Mappers
{
    public static class NoteCategoryMapper
    {
        public static NoteCategoryViewModel MapFrom(this NoteGategoryDTO dto)
        {
            return new NoteCategoryViewModel()
            {
                CategoryId = dto.Id,
                CategoryName = dto.Name
            };
        }
    }
}
