using ManagerLogbook.Web.Areas.Manager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class IndexNoteViewModel
    {
        public int? PrevPage { get; set; }

        public int CurrPage { get; set; }

        public int? NextPage { get; set; }

        public int TotalPages { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        //public int? CategoryId { get; set; }
        //public IEnumerable<SelectListItem> Categories { get; set; }
        //public string CategoryName { get; set; }

        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }

        public int? CategoryId { get; set; }
        public IReadOnlyCollection<NoteCategoryViewModel> Categories { get; set; }
        public string CategoryName { get; set; }

        public IReadOnlyCollection<LogbookViewModel> Logbooks { get; set; }
    }
}
