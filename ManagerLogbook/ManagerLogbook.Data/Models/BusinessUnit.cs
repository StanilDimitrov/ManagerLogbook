using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class BusinessUnit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BrandName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Picture { get; set; }

        public ICollection<Logbook> Logbook { get; set; }
    }
}
