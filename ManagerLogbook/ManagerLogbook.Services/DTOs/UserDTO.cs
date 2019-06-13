namespace ManagerLogbook.Services.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Picture { get; set; }
       
        public string BusinessUnitName { get; set; }

        public int? BusinessUnitId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? CurrentLogbookId { get; set; }
    }
}
