using System.Collections.Generic;

namespace ManagerLogbook.Services.DTOs
{
    public class LogbookDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string BusinessUnitName { get; set; }

        public IReadOnlyCollection<NoteDTO> Notes { get; set; }
    }
}
