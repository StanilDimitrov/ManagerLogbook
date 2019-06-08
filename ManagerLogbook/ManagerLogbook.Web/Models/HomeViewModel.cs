using ManagerLogbook.Web.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class HomeViewModel
    {
        public IReadOnlyCollection<BusinessUnitViewModel> BusinessUnits { get; set; }

        public IReadOnlyCollection<TownViewModel> Towns { get; set; }

        public IReadOnlyCollection<BusinessUnitCategoryViewModel> Categories { get; set; }

        public NoteViewModel Note { get; set; }

        public RegisterViewModel Register { get; set; }

        public LoginViewModel Login { get; set; }

        public FooterViewModel Footer { get; set; }
         
        public BusinessUnitViewModel BusinessUnit { get; set; }

        public BusinessUnitSearch SearchModelBusiness { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int TownId { get; set; }
        public string TownName { get; set; }
    }
}
