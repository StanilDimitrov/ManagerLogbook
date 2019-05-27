using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Data.Models
{
    public class BusinessUnitCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<BusinessUnit> BusinessUnits { get; set; }
    }
}
