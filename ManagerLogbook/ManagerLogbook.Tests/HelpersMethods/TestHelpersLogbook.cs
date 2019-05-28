using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public static class TestHelpersLogbook
    {
        public static Logbook TestLogbook01()
        {
            return new Logbook
            {
                Id = 1,
                Name = new string('a', 49),
                Picture = "pictureLogbook01",
                BusinessUnitId = 1
            };
        }

        public static Logbook TestLogbook02()
        {
            return new Logbook
            {
                Id = 2,
                Name = new string('a', 501),
                Picture = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                BusinessUnitId = 1
            };
        }

        public static Logbook TestLogbook03()
        {
            return new Logbook
            {
                Id = 3,
                Name = "Hotel",
                Picture = "pictureHotel",
                BusinessUnitId = 1
            };
        }

        public static BusinessUnit TestBusinessUnit01()
        {
            return new BusinessUnit
            {
                Id = 1,
                Name = "Hilton",
                Address = "Cherni Vryh 15",
                PhoneNumber = "0123456789",
                Email = "info@hilton.com"
            };
        }

        public static Note TestNote01()
        {
            return new Note
            {
                Id = 1,
                Description = "Room 37 is dirty",
            };
        }

        public static Note TestNote02()
        {
            return new Note
            {
                Id = 2,
                Description = "Room 37 is dirty",
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
            };
        }

        public static User TestUser02()
        {
            return new User
            {
                Id = "e92f6f7d-87cc-428d-b3bc-b096a227c109",
                UserName = "peter",
                Email = "abv@bg",
                PasswordHash = "654321"
            };
        }

        public static UsersLogbooks TestUsersLogbooks01()
        {
            return new UsersLogbooks
            {
                UserId = "70c946ce-0c88-4084-ab2e-dfbf53783c05",
                LogbookId = 1,
            };
        }

        public static UsersLogbooks TestUsersLogbooks02()
        {
            return new UsersLogbooks
            {
                UserId = "4e28316d-5870-4be1-8564-0c6b1415789d",
                LogbookId = 1
            };
        }

        public static UsersLogbooks TestUsersLogbooks03()
        {
            return new UsersLogbooks
            {
                UserId = "70c946ce-0c88-4084-ab2e-dfbf53783c05",
                LogbookId = 3,
            };
        }

        public static NoteCategory TestNoteCategory1()
        {
            return new NoteCategory
            {
                Id = 1,
                Name = "Restorant stuff"
            };
        }

        public static Note TestNote2()
        {
            return new Note
            {
                Id = 2,
                Description = "Room 37 is dirty and need to be clean",
                User = TestUser01(),
                UserId = TestUser01().Id,
                Logbook = TestLogbook01(),
                CreatedOn = DateTime.Now.AddDays(-2),
                NoteCategory = TestNoteCategory1(),
                NoteCategoryId = TestNoteCategory1().Id
            };
        }
    }
}
