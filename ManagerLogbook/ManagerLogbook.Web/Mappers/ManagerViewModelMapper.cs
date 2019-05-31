using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Mappers
{
    public static class ManagerViewModelMapper
    {
        public static ManagerViewModel MapFrom(this UserDTO dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new ManagerViewModel()
            {
                CurrentLogbookId = dto.CurrentLogbookId,
                Email = dto.Email,
                Avatar = dto.Picture,
                UserName = dto.UserName
            };
        }
    }
}
