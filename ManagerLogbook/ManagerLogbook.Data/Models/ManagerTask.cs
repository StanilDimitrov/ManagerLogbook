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
        [MaxLength(500)]
        public string Description { get; set; }
                
        [MaxLength(50)]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public int? TaskCategoryId { get; set; }
        public TaskCategory TaskCategory { get; set; }        

        public int LogbookId { get; set; }
        public Logbook Logbook { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
