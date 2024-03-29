﻿using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelpersNoteController
    {
        public static NoteViewModel TestNoteViewModel1()
        {
            return new NoteViewModel
            {
                Description = "Room 37 is dirty",
                Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                CreatedOn = DateTime.Now.AddDays(-2),
            };
        }

        public static NoteDTO TestNoteDTO1()
        {
            return new NoteDTO
            {
                Description = "Room 37 is dirty",
                Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
                CreatedOn = DateTime.Now.AddDays(-2),
            };
        }

        public static NoteDTO TestNoteDTO2()
        {
            return new NoteDTO
            {
                Description = "Room 37 is dirty",
                Image = null,
                CreatedOn = DateTime.Now.AddDays(-2),
                CategoryId = null,
                UserId = TestUserDTO1().Id
            };
        }

        public static NoteDTO TestNoteDTO3()
        {
            return new NoteDTO
            {
                Id = 3,
                Description = "Room 37.",
                Image = null,
                CreatedOn = DateTime.Now.AddDays(-2),
                CategoryId = null,
                UserId = TestUserDTO1().Id,
                IsActiveTask = true
            };
        }

        public static NoteDTO TestNoteDTO4()
        {
            return new NoteDTO
            {
                Id = 4,
                Description = "Room 37.",
                Image = null,
                CreatedOn = DateTime.Now.AddDays(-2),
                CategoryId = null,
                UserId = TestUserDTO1().Id,
                IsActiveTask = true,
            };
        }

        public static NoteDTO TestNoteDTO5()
        {
            return new NoteDTO
            {
                Id = 5,
                Description = "Room 37.",
                Image = null,
                CreatedOn = DateTime.Now.AddDays(-2),
                CategoryId = null,
                UserId = TestUserDTO1().Id,
                IsActiveTask = false,
            };
        }

        public static UserDTO TestUserDTO1()
        {
            return new UserDTO
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
                CurrentLogbookId = 1,
                BusinessUnitId = 1,
            };
        }

        public static UserDTO TestUserDTO2()
        {
            return new UserDTO
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
                CurrentLogbookId = 1
            };
        }

        public static UserDTO TestUserDTO3()
        {
            return new UserDTO
            {
                Id = "c2fb4e2d-c6f6-43f2-ac26-b06ef1113981",
                UserName = "ivan",
                Email = "dir@bg",
            };
        }

        public static LogbookDTO TestLogbookDTO1()
        {
            return new LogbookDTO
            {
                Id = 1,
                Name = "Hotel"
            };
        }

        public static LogbookDTO TestLogbookDTO2()
        {
            return new LogbookDTO
            {
                Id = 2,
                Name = "Restaurant"
            };
        }

        public static NoteGategoryDTO TestNoteCategoryDTO1()
        {
            return new NoteGategoryDTO
            {
                Id = 1,
                Name = "Task"
            };
        }
    }
}
