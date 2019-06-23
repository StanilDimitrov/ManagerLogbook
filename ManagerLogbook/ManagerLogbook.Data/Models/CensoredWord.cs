using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Data.Models
{
    public class CensoredWord
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Word { get; set; }
    }
}
