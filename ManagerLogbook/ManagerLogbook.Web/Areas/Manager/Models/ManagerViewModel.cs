using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Manager.Models
{
    public class ManagerViewModel
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public IFormFile AvatarImage { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? CurrentLogbookId { get; set; }
        public IEnumerable<SelectListItem> Logbooks { get; set; }
        public string LogbookName { get; set; }
    }
}
