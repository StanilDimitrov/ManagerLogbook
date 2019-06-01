using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class HomeViewModel
    {
        public IReadOnlyCollection<BusinessUnitViewModel> BusinessUnits { get; set; }

        public int CitiesCount { get; set; }

        public int HotelsCount { get; set; }

        public int RestaurantsCount { get; set; }
    }
}
