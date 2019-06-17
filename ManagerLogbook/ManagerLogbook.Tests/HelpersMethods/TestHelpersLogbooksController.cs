using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelpersLogbookController
    {
        public static LogbookViewModel TestLogbookViewModel01()
        {
            return new LogbookViewModel
            {
                Name = "Logbook01",
                BusinessUnitId = 1,
                Picture = "1"
            };
        }

        public static LogbookDTO TestLogbookDTO01()
        {
            return new LogbookDTO
            {
                Name = "Logbook01",
                BusinessUnitName = "Bu01",
                Picture = null
            };
        }
    }
}
