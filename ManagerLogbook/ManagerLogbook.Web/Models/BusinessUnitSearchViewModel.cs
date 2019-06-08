using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class BusinessUnitSearchViewModel
    {
        public IReadOnlyCollection<BusinessUnitViewModel> BusinessUnits { get; set; }
    }
}
