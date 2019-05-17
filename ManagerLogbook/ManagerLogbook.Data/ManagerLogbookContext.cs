using ManagerLogbook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data
{
    public class ManagerLogbookContext : IdentityDbContext<ApplicationUser>
    {
        public ManagerLogbookContext(DbContextOptions options) : base(options)
        {

        }
    }
}
