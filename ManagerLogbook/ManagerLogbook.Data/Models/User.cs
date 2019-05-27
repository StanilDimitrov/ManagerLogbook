using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ManagerLogbook.Data.Models
{
    public class User : IdentityUser
    {
        public string Picture { get; set; }   
        
        public int? CurrentLogbookId { get; set; }

        public ICollection<Note> Notes { get; set; }

        public int? BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public ICollection<UsersLogbooks> UsersLogbooks { get; set; }
    }
}
