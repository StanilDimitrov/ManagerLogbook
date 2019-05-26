using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.DTOs
{
    public class LogbookDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string BusinessUnitName { get; set; }
    }
}
