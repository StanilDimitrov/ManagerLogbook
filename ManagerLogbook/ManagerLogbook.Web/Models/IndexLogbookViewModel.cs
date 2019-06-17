using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models.AccountViewModels;
using System.Collections.Generic;

namespace ManagerLogbook.Web.Models
{
    public class IndexLogbookViewModel
    {
        public RegisterViewModel Register { get; set; }

        public LoginViewModel Login { get; set; }

        public FooterViewModel Footer { get; set; }

        public BusinessUnitViewModel BusinessUnit { get; set; }

        public NoteViewModel Note { get; set; }

        public LogbookViewModel Logbook { get; set; }

        public IReadOnlyCollection<UserDTO> AssignedManagers { get; set; }

        public IReadOnlyCollection<NoteDTO> ActiveNotes { get; set; }

        public IReadOnlyCollection<NoteDTO> TotalNotes { get; set; }
    }
}
