namespace ManagerLogbook.Services.Models
{
    public class LogbookModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CurrentLogbookId { get; set; }

        public string Picture { get; set; }
  
        public int BusinessUnitId { get; set; }

        public string ManagerId { get; set; }
        
    }
}
