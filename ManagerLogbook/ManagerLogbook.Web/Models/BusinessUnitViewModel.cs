using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Models
{
    public class BusinessUnitViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Information { get; set; }

        [Required(ErrorMessage = "Name must be between 2 and 100 characters long.")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }           
        
        public string Picture { get; set; }

        public IFormFile BusinessUnitPicture { get; set; }

        public int Likes { get; set; }

        public string ModeratorId { get; set; }
        public IEnumerable<SelectListItem> Moderators { get; set; }
        public string ModeratorName { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string CategoryName { get; set; }

        [Required]
        public int? TownId { get; set; }
        public IEnumerable<SelectListItem> Towns { get; set; }
        public string TownName { get; set; }        

        public ReviewViewModel Review { get; set; }
    }
}
