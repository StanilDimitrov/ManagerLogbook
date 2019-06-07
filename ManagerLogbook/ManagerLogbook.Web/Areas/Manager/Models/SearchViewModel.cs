using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Manager.Models
{
    public class SearchViewModel
    {
        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }
    }
}
