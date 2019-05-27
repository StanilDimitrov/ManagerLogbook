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

            return new UserDTO()
            {
                Picture = entity.Picture,
                BusinessUnitName = entity.BusinessUnit.Name,
                UserName = entity.UserName,
                Email = entity.Email,
                //CurrentLogbookId = entity.CurrentLogbookId
            };
        }
    }
}
