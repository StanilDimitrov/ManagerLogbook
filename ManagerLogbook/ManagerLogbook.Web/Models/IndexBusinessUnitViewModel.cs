using ManagerLogbook.Web.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Models
{
    public class IndexBusinessUnitViewModel
    {       
        public NoteViewModel Note { get; set; }

        public RegisterViewModel Register { get; set; }

        public LoginViewModel Login { get; set; }

        public FooterViewModel Footer { get; set; }
         
        public BusinessUnitViewModel BusinessUnit { get; set; }

        public IReadOnlyCollection<ReviewViewModel> Reviews { get; set; }
    }
}
