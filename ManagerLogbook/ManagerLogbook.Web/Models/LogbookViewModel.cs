using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Models
{
    public class LogbookViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? CurrentLogbookId { get; set; }

        public string Picture { get; set; }
        public IFormFile LogbookPicture { get; set; }

        public string BusinessUnitName { get; set; }
        public IEnumerable<SelectListItem> BusinessUnits { get; set; }
        public int BusinessUnitId { get; set; }

        public string ManagerId { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
        public string ManagerName { get; set; }

        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }

        public IReadOnlyCollection<LogbookViewModel> UserLogbooks { get; set; }
    }
}
