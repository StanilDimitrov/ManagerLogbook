using ManagerLogbook.Data.Models;
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
                LogbookId = TestLogbook1().Id,
                UserId = TestUser1().Id,
                Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                CreatedOn = DateTime.Now,
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
                Logbook = TestLogbook1(),
                CreatedOn = DateTime.Now.AddDays(-2)
            };
        }

        public static Note TestNote3()
        {
            return new Note
            {
                Id = 3,
                Description = "Room 27 is dirty and need to be clean",
                User = TestUser1(),
                Logbook = TestLogbook1(),
                CreatedOn = DateTime.Now.AddDays(-5)
            };
        }

        public static Note TestNote4()
        {
            return new Note
            {
                Id = 1,
                Description = "Room 37 is dirty",
                User = TestUser1(),
                LogbookId = TestLogbook1().Id,
                UserId = TestUser1().Id,
                NoteCategory = TestNoteCategory2(),
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

        public static NoteCategory TestNoteCategory1()
        {
            return new NoteCategory
            {
                Id = 1,
                Type = "Restorant stuff"
            };
        }

        public static NoteCategory TestNoteCategory2()
        {
            return new NoteCategory
            {
                Id = 2,
                Type = "Task"
            };
        }

        public static User TestUser1()
        {
            return new User
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
                PasswordHash = "123456"
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

        public static Logbook TestLogbook1()
        {
            return new Logbook
            {
                Id = 1,
            };
        }
    }
}
