using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string OriginalComment { get; set; }

        public string EditedComment { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
