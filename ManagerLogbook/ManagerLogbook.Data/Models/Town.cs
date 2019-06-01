using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Town
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<BusinessUnit> BusinessUnits { get; set; }
    }
}
