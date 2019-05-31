using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class NoteCategoryDTOMapper
    {
        public static NoteGategoryDTO ToDTO(this NoteCategory entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new NoteGategoryDTO()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
