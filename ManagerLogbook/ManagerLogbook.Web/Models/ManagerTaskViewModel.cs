using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class ManagerTaskViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Note { get; set; }

        public DateTime CreatedOn { get; set; }

        public IFormFile TaskImage { get; set; }

        public string Image { get; set; }

        public string Status { get; set; }

        public string User { get; set; }
    }
}
