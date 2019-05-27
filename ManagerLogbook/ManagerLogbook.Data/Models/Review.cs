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

        [Required]
        [Range(0, 5,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool isVisible { get; set; }

        public int BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }
}
