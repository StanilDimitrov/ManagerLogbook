namespace ManagerLogbook.Services.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public string OriginalDescription { get; set; }

        public string EditedDescription { get; set; }

        public int Rating { get; set; }

        public int BusinessUnitId { get; set; }
    }
}
