using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Data.Models
{
    public class User : IdentityUser
    {
        public string Picture { get; set; }        

        public ICollection<Note> Notes { get; set; }

        public int? BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public ICollection<UsersLogbooks> UsersLogbooks { get; set; }
    }
}
