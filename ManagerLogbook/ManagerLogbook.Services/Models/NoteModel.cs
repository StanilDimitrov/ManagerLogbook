namespace ManagerLogbook.Services.Models
{
    public class NoteModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int? CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
