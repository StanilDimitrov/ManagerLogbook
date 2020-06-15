namespace ManagerLogbook.Services.Models
{
    public class BusinessUnitModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public int CategoryId { get; set; }

        public int TownId { get; set; }
    }
}
