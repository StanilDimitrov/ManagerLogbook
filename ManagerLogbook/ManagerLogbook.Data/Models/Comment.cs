using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string OriginalDescription { get; set; }

        public string EditedDesctiption { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
