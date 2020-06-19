using System;

namespace ManagerLogbook.Services.Models
{
    public class SearchNoteModel
    {
        public string UserId { get; set; }

        public int LogbookId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? CategoryId { get; set; }

        public string SearchCriteria { get; set; }

        public int? DaysBefore { get; set; }

        public int CurrPage { get; set; } = 1;
    }
}
