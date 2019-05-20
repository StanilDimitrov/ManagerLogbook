using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Logbook
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Picture { get; set; }

        public int BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public int LogbookCategoryId { get; set; }
        public LogbookCategory LogbookCategory { get; set; }

        public ICollection<UsersLogbooks> UsersLogbooks { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<ManagerTask> ManagerTasks { get; set; }
    }

}
