using ManagerLogbook.Web.Models;
using System.Collections.Generic;

namespace ManagerLogbook.Web.Areas.Moderator.Models
{
    public class IndexReviewViewModel
    {
        public ICollection<ReviewViewModel> Reviews { get; set; }

        public BusinessUnitViewModel BusinessUnit { get; set; }

        public string ModeratorId { get; set; }

        public int? PrevPage { get; set; }

        public int CurrPage { get; set; }

        public int? NextPage { get; set; }

        public int TotalPages { get; set; }
    }
}
