using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public static class TestHelpersNote
    {
        public static Note TestNote1()
        {
            return new Note
            {
                Id = 1,
                Description = "Room 37 is dirty",
                User = TestUser1(),
                NoteCategory = TestNoteCategory2(),
                LogbookId = TestLogbook1().Id,
                UserId = TestUser1().Id,
                Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                CreatedOn = DateTime.Now.AddDays(-2),
                NoteCategoryId = TestNoteCategory2().Id
            };
        }

        public static Note TestNote2()
        {
            return new Note
            {
                Id = 2,
                Description = "Room 37 is dirty and need to be clean",
                User = TestUser1(),
                UserId = TestUser1().Id,
                Logbook = TestLogbook1(),
                CreatedOn = DateTime.Now.AddDays(-2),
                NoteCategory = TestNoteCategory2(),
                NoteCategoryId = TestNoteCategory2().Id
            };
        }

        public static Note TestNote3()
        {
            return new Note
            {
                Id = 3,
                Description = "Room 27 is dirty and need to be clean",
                User = TestUser1(),
                UserId = TestUser1().Id,
                Logbook = TestLogbook1(),
                CreatedOn = DateTime.Now.AddDays(-10),
                NoteCategory = TestNoteCategory3(),
                NoteCategoryId = TestNoteCategory3().Id
            };
        }

        public static Note TestNote4()
        {
            return new Note
            {
                Id = 1,
                Description = "Room 37 is dirty",
                User = TestUser1(),
                UserId = TestUser1().Id,
                LogbookId = TestLogbook1().Id,
                NoteCategoryId = TestNoteCategory2().Id,
                IsActiveTask = true
            };
        }

        public static UsersLogbooks TestUsersLogbooks1()
        {
            return new UsersLogbooks
            {
                UserId = TestUser1().Id,
                LogbookId = TestLogbook1().Id
            };
        }

        public static UsersLogbooks TestUsersLogbooks2()
        {
            return new UsersLogbooks
            {
                UserId = TestUser2().Id,
                LogbookId = TestLogbook2().Id
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

        public static NoteCategory TestNoteCategory2()
        {
            return new NoteCategory
            {
                Id = 2,
                Name = "Task"
            };
        }

        public static NoteCategory TestNoteCategory3()
        {
            return new NoteCategory
            {
                Id = 3,
                Name = "Task"
            };
        }


        public static User TestUser1()
        {
            return new User
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
                PasswordHash = "123456",
                BusinessUnitId = 1,
                
            };
        }

        public static User TestUser2()
        {
            return new User
            {
                Id = "41cf52c4-dcda-40bf-b743-7692fb8a37b9",
                UserName = "peter",
                Email = "dnes@bg",
                PasswordHash = "123456"
            };
        }

        public static User TestUser3()
        {
            return new User
            {
                Id = "6422552b-c251-491c-bcff-b2216430b6e4",
                UserName = "georgi",
                Email = "gol@bg",
                PasswordHash = "123456",
            };
        }

        public static Logbook TestLogbook1()
        {
            return new Logbook
            {
                Id = 1,
                Name = "Restaurant Logbook"
            };
        }

        public static Logbook TestLogbook2()
        {
            return new Logbook
            {
                Id = 1,
                Name = "Restaurant Marinela"
            };
        }

        public static NoteDTO TestNoteDTO1()
        {
            return new NoteDTO()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                UserName = "ivan",
                LogbookName = "Restaurant Logbook",
                IsActiveTask = true,
                Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                CreatedOn = DateTime.Now,
                CategoryName = "Task"
            };
        }
    }
}
