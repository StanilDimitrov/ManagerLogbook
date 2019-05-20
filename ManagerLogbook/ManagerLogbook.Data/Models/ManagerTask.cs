using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class ManagerTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Note { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string Image { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

    }
}
