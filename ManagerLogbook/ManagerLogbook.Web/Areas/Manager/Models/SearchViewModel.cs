using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Manager.Models
{
    public class SearchViewModel
    {
        public int? PrevPage { get; set; }

        public int CurrPage { get; set; }

        public int? NextPage { get; set; }

        public int TotalPages { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? CategoryId { get; set; }

        public string SearchCriteria { get; set; }

        public int ScrollPage { get; set; }

        public string SkipNotesForScrollResult { get; set; }

        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }
    }
}
