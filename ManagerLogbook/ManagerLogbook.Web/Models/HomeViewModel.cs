using ManagerLogbook.Web.Models.AccountViewModels;
using System.Collections.Generic;

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

        public LogbookViewModel Logbook { get; set; }

        public BusinessUnitSearchViewModel SearchModelBusiness { get; set; }
    }
}
