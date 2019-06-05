using ManagerLogbook.Web.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class IndexNoteViewModel
    {
        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }

        public IReadOnlyCollection<NoteCategoryViewModel> Categories { get; set; }

        public IReadOnlyCollection<LogbookViewModel> Logbooks { get; set; }
    }
}
