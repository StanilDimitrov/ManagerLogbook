using ManagerLogbook.Services.DTOs;
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

    }
}
