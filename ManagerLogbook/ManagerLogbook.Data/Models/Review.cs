using System;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string OriginalDescription { get; set; }

        [Required]
        [MaxLength(500)]
        public string EditedDescription { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool isVisible { get; set; }

        public int BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }
}
