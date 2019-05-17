using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Note { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
