using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class NoteDTOMapper
    {
        public static NoteDTO ToDTO(this Note entity)
        {
            if (entity is null)
            {
                return null;
            }

            var dto = new NoteDTO()
            {
                Id = entity.Id,
                Description = entity.Description,
                IsActiveTask = entity.IsActiveTask,
                Image = entity.Image,
                CreatedOn = entity.CreatedOn,
                UserName = entity.User?.UserName,
                UserId = entity.UserId,
                CategoryId = entity.NoteCategoryId,
                CategoryName = entity.NoteCategory?.Name
            };

            return dto;
        }
    }
}
