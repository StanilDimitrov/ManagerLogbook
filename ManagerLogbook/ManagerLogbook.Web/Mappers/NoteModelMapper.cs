using ManagerLogbook.Services.Models;
using ManagerLogbook.Web.Models;

namespace ManagerLogbook.Web.Mappers
{
    public static class NoteModelMapper
    {
        public static NoteModel MapFrom(this NoteViewModel viewModel)
        {
            return new NoteModel()
            {
                Id = viewModel.Id,
                Description = viewModel.Description,
                Image = viewModel.Image,
                CategoryId = viewModel.CategoryId,
               
            };
        }
    }
}
