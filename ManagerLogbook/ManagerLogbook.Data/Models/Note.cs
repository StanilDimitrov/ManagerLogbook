using System;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Data.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image { get; set; }

        public bool IsActiveTask { get; set; }

        public int? NoteCategoryId { get; set; }
        public NoteCategory NoteCategory { get; set; }

        public int LogbookId { get; set; }
        public Logbook Logbook { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
