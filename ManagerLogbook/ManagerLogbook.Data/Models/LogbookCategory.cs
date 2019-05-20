using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class LogbookCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        public ICollection<Logbook> Logbook { get; set; }
    }
}
