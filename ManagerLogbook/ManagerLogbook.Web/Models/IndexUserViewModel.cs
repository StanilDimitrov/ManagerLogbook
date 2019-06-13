using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class IndexUserViewModel
    {
        public UserViewModel User { get; set; }

        public LogbookViewModel Logbook { get; set; }

        public NoteViewModel Note { get; set; }

        public BusinessUnitViewModel BusinessUnit { get; set; }

        public FooterViewModel Footer { get; set; }
    }
}
