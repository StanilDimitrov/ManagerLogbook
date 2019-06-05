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
                Name = "Kempinski",
                Address = "Cherni Vryh 15",
                PhoneNumber = "1111111111",
                Email = "info@kempinski.com",
                Information="This is information for BU01",
                BusinessUnitCategoryId=1,
                TownId = 1
                
            };
        }

        public static BusinessUnit TestBusinessUnit02()
        {
            return new BusinessUnit
            {
                Id = 2,
                Name = "Hilton",
                Address = "Cherni Vryh 95",
                PhoneNumber = "0123456789",
                Email = "info@hilton.com",
                Information = "This is information for BU02",
                BusinessUnitCategoryId = 1,
                TownId = 1
            };
        }

        public static BusinessUnitCategory TestBusinessUnitCategory01()
        {
            return new BusinessUnitCategory
            {
                Id = 1,
                Name = "Hotel"
            };
        }

        public static BusinessUnitCategory TestBusinessUnitCategory02()
        {
            return new BusinessUnitCategory
            {
                Id = 2,
                Name = "Restaurant"
            };
        }

        public static BusinessUnitCategory TestBusinessUnitCategory03()
        {
            return new BusinessUnitCategory
            {
                Id = 3,
                Name = "Spa Center"
            };
        }

        public static Note TestNote01()
        {
            return new Note
            {
                Id = 1,
                Description = "Room 37 is dirty",
                //User = TestUser1(),
                //NoteCategory = TestNoteCategory2(),
                LogbookId = 1,
                //UserId = TestUser1().Id,
                //Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                //CreatedOn = DateTime.Now.AddDays(-2),
                //NoteCategoryId = TestNoteCategory2().Id
            };
        }

        public static Town TestTown01()
        {
            return new Town
            {
                Id = 1,
                Name = "Sofia"
            };
        }

        public static Town TestTown02()
        {
            return new Town
            {
                Id = 2,
                Name = "Varna"
            };
        }

        public static User TestUser01()
        {
            return new User
            {
                Id = "70c946ce-0c88-4084-ab2e-dfbf53783c05",
                UserName = "ivan",
                Email = "dir@bg",
                PasswordHash = "123456",
                BusinessUnitId = 1
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
