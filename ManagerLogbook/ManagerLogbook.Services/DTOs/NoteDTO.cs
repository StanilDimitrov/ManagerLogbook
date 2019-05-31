using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ManagerLogbook.Services.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image { get; set; }

        public IFormFile NoteImage { get; set; }

        public int? CategoryId { get; set; }

        public bool IsActiveTask { get; set; }

        public string NoteCategoryName { get; set; }

        public string LogbookName { get; set; }

        public string UserName { get; set; }

        public string UserId{ get; set; }
    }
}
