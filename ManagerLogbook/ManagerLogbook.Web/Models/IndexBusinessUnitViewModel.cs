﻿using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models.AccountViewModels;
using System.Collections.Generic;

namespace ManagerLogbook.Web.Models
{
    public class IndexBusinessUnitViewModel
    {       
        public NoteViewModel Note { get; set; }

        public RegisterViewModel Register { get; set; }

        public LoginViewModel Login { get; set; }

        public LogbookViewModel Logbook { get; set; }

        public BusinessUnitViewModel BusinessUnit { get; set; }

        public IReadOnlyCollection<ReviewViewModel> Reviews { get; set; }

        public IReadOnlyCollection<LogbookDTO> Logbooks { get; set; }
    }
}
