using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Models
{
    public class BusinessUnitViewModel
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

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Picture { get; set; }

        public string BusinessUnitCategoryName { get; set; }
    }
}
