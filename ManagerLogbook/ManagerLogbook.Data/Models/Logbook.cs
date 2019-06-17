using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public ICollection<Note> Notes { get; set; }

        public ICollection<UsersLogbooks> UsersLogbooks { get; set; }
    }

}
