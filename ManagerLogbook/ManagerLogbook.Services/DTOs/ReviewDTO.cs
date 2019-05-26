using System;

namespace ManagerLogbook.Services.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        public string OriginalDescription { get; set; }

        public string EditedDescription { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool isVisible { get; set; }
       
        public string BusinessUnitName { get; set; }
    }
}
