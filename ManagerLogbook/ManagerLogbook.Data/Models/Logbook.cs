using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Logbook
    {
        public int Id { get; set; }
               
        public int SubBusinessUnitId { get; set; }

        public SubBusinessUnit SubBusinessUnit { get; set; }

        public ICollection<UsersLogbooks> UsersLogBooks { get; set; }
    }

}
