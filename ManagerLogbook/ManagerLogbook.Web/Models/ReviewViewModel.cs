using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string OriginalDescription { get; set; }
                
        [MaxLength(500)]
        public string EditedDescription { get; set; }
                
        //[Range(0, 5,
        //ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool isVisible { get; set; }

        public string BusinessUnitName { get; set; }

        public int BusinessUnitId { get; set; }
    }
}
