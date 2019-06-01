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

        public DbSet<BusinessUnitCategory> BusinessUnitCategories { get; set; }

        public DbSet<CensoredWord> CensoredWords { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Logbook> Logbooks { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<UsersLogbooks> UsersLogbooks { get; set; }

        public DbSet<NoteCategory> NoteCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersLogbooksConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
