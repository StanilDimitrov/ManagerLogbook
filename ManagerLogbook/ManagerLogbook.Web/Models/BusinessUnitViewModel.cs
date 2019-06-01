using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Models
{
    public class BusinessUnitViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Information { get; set; }

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

        public string BusinessUnitCategoryName { get; set; }

        public string BusinessUnitTownName { get; set; }
    }
}
