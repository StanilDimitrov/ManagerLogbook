using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;

namespace ManagerLogbook.Services.Mappers
{
    public static class NoteCategoryDTOMapper
    {
        public static NoteGategoryDTO ToDTO(NoteCategory entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new NoteGategoryDTO()
            {
                Id = entity.Id,
                Type = entity.Type
            };
        }
    }
}
