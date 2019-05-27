using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Manager.Models
{
    public class ManagerViewModel
    {
        public string Picture { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? CurrentLogbookId { get; set; }
    }
}
