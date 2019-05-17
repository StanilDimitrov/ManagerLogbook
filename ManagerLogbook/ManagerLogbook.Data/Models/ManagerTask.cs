using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class ManagerTask
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime CreatedOn { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
