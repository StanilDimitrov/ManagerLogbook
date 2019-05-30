using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Data.Models
{
    public class BusinessUnit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Picture { get; set; }

        public int BusinessUnitCategoryId { get; set; }
        public BusinessUnitCategory BusinessUnitCategory { get; set; }

        public ICollection<Logbook> Logbooks { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<CensoredWord> CensoredWords { get; set; }
    }
}
