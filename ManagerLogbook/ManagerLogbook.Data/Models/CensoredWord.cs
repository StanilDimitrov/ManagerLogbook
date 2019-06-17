using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class CensoredWord
    {
        public int Id { get; set; }

        public string Word { get; set; }

        public int BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }
}
