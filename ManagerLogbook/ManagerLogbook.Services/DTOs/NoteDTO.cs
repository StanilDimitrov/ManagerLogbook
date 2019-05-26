using System;

namespace ManagerLogbook.Services.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image { get; set; }

        public bool IsActiveTask { get; set; }

        public string NoteCategoryType { get; set; }

        public string LogbookName { get; set; }

        public string UserName { get; set; }
    }
}
