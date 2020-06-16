﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Models
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image { get; set; }

        public IFormFile NoteImage { get; set; }

        public bool IsActiveTask { get; set; }

        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string CategoryName { get; set; }
      
        public string UserName { get; set; }

        public bool CanUserEdit { get; set; }

        public string UserId { get; set; }
    }
}
