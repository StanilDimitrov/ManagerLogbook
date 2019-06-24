using ManagerLogbook.Data.Models;
using System.Collections.Generic;

namespace ManagerLogbook.Services.DTOs
{
    public class BusinessUnitDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Information { get; set; }

        public string Picture { get; set; }

        public int Likes { get; set; }

        public string CategoryName { get; set; }

        public string BusinessUnitTownName { get; set; }
    }
}
