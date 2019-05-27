using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public static class TestHelperBusinessUnit
    {
        public static BusinessUnit TestBusinessUnit01()
        {
            return new BusinessUnit
            {
                Id = 1,
                Name="Hilton",
                Address="Cherni Vryh 15",
                PhoneNumber="0123456789",
                Email="info@hilton.com"
            };
        }

        public static Logbook TestLogbook01()
        {
            return new Logbook
            {
                Id = 1,
                Name = "NameOfLogbook",
                Picture = "PathToThePicture"
            };
        }

        public static Logbook TestLogbook02()
        {
            return new Logbook
            {
                Id = 2,
                Name = "NameOfLogbook2",
                Picture = "PathToThePicture2",
                BusinessUnitId = 1
            };
        }

        public static Logbook TestLogbook03()
        {
            return new Logbook
            {
                Id = 3,
                Name = "NameOfLogbook3",
                Picture = "PathToThePicture3",
                BusinessUnitId = 1
            };
        }
    }
}
