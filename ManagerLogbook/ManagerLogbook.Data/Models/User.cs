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
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string Picture { get; set; }

        public ICollection<UsersLogbooks> UsersLogbooks { get; set; }

        public ICollection<ManagerTask> ManagerTasks { get; set; }
    }
}
