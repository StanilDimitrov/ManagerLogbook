using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class BusinessUnit
    {
        public int Id { get; set; }

        public string BrandName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public ICollection<SubBusinessUnit> subBusinessUnit { get; set; }        
    }
}
