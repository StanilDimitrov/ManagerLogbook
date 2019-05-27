﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerLogbook.Web.Areas.Manager.Models
{
    public class LogbookViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? CurrentLogbookId { get; set; }

        public string Picture { get; set; }

        public string BusinessUnitName { get; set; }

        public IReadOnlyCollection<NoteViewModel> Notes { get; set; }
    }
}