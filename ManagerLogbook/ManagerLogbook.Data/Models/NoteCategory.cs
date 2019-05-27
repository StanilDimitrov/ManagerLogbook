using System.Collections.Generic;

namespace ManagerLogbook.Data.Models
{
    public class NoteCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}
