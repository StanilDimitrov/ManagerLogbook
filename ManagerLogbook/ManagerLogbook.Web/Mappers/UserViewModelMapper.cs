using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Mappers
{
    public static class UserViewModelMapper
    {
        public static UserViewModel MapFrom(this UserDTO dto)
        {
            return new UserViewModel()
            {
                Id = dto.Id,
                UserName = dto.UserName,
                Avatar = dto.Picture,
                Email = dto.Email,
                CurrentLogbookId = dto.CurrentLogbookId,
                BusinessUnitId = dto.BusinessUnitId
            };
        }
    }
}
