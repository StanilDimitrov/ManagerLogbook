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
        [MinLength(5)]
        public string BrandName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Picture { get; set; }

        public ICollection<SubBusinessUnit> subBusinessUnit { get; set; }
    }
}
