using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ManagerLogbook.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public IFormFile AvatarImage { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? CurrentLogbookId { get; set; }
        public IEnumerable<SelectListItem> Logbooks { get; set; }
        public string LogbookName { get; set; }

        public string UserRole { get; set; }

        public int? BusinessUnitId { get; set; }
    }
}
