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
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SearchCriteria { get; set; }

        public LogbookViewModel CurrentLogbook { get; set; }

        public UserViewModel Manager { get; set; }

        //public string CurrentLogbookName { get; set; }

        public int? CurrentLogbookId { get; set; }

        public NoteViewModel Note { get; set; }


        //public int? CategoryId { get; set; }
        //public IEnumerable<SelectListItem> Categories { get; set; }
        //public string CategoryName { get; set; }

        public SearchViewModel SearchModel { get; set; }

        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }

        

        public IReadOnlyCollection<LogbookViewModel> Logbooks { get; set; }
    }
}
