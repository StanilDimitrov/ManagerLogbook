using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Moderator.Models
{
    public class IndexReviewViewModel
    {
        public ICollection<ReviewViewModel> Reviews { get; set; }

        public BusinessUnitViewModel BusinessUnit { get; set; }
    }
}
