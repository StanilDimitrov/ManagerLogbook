using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class NoteViewModelMapper
    {
        public static NoteViewModel MapFrom(this NoteDTO dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new NoteViewModel()
            {
                Description = dto.Description,
                Image = dto.Image,
                UserName = dto.UserName,
                IsActiveTask = dto.IsActiveTask,
                NoteCategoryName = dto.NoteCategoryType,
                CreatedOn = dto.CreatedOn
            };
        }
    }
}
