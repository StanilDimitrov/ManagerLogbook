using System.Collections.Generic;

namespace ManagerLogbook.Services.DTOs
{
    public class NoteGategoryDTO
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public IReadOnlyCollection<BusinessUnitDTO> BusinessUnits { get; set; }
    }
}
