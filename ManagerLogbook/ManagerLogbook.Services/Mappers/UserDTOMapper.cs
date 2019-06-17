using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class UserDTOMapper
    {
        public static UserDTO ToDTO(this User entity)
        {
            if (entity is null)
            {
                return null;
            }

            var user =  new UserDTO()
            {
                Id = entity.Id,
                Picture = entity.Picture,
                BusinessUnitName = entity.BusinessUnit?.Name,
                BusinessUnitId = entity.BusinessUnitId,
                UserName = entity.UserName,
                Email = entity.Email,
                CurrentLogbookId = entity.CurrentLogbookId
            };

            return user;
        }
    }
}
