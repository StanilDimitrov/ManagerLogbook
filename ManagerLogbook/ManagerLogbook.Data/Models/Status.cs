using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Status
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public ICollection<ManagerTask> Tasks { get; set; }
    }
}
