using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class BusinessUnitSearch
    {
        public IReadOnlyCollection<BusinessUnitViewModel> BusinessUnits { get; set; }
    }
}
