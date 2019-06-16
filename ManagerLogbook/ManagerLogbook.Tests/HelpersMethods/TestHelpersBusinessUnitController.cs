using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelpersBusinessUnitController
    {
        public static BusinessUnitViewModel TestBusinessUnitViewModel01()
        {
            return new BusinessUnitViewModel
            {
                Name = "BusinessUnit01",
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };
        }

        public static BusinessUnitDTO TestBusinessUnitDTO01()
        {
            return new BusinessUnitDTO
            {
                Name = "BusinessUnit01",
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information"
            };
        }
    }
}
