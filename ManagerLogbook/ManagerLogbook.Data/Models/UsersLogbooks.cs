using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class UsersLogbooks
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int LogbookId { get; set; }
        public Logbook Logbook { get; set; }
    }
}
