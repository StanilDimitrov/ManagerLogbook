using ManagerLogbook.Data.EntityConfiguration;
using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagerLogbook.Data
{
    public class ManagerLogbookContext : IdentityDbContext<User>
    {
        public ManagerLogbookContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BusinessUnit> BusinessUnits { get; set; }        

        public DbSet<Logbook> Logbooks { get; set; }

        public DbSet<LogbookCategory> LogbookCategories { get; set; }

        public DbSet<ManagerTask> ManagerTasks { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<TaskCategory> TaskCategories { get; set; }

        public DbSet<UsersLogbooks> UsersLogbooks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersLogbooksConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
