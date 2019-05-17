using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class SubBusinessUnit
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public ICollection<Logbook> Logbooks { get; set; }

    }
}
