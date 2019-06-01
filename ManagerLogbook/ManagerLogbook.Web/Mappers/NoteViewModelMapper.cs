using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class NoteViewModelMapper
    {
        public static NoteViewModel MapFrom(this NoteDTO dto)
        {
            return new NoteViewModel()
            {
                Id= dto.Id,
                Description = dto.Description,
                Image = dto.Image,
                UserName = dto.UserName,
                UserId = dto.UserId,
                IsActiveTask = dto.IsActiveTask,
                CategoryName = dto.NoteCategoryName,
                CreatedOn = dto.CreatedOn
            };
        }
    }
}
