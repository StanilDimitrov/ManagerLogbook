using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using ManagerLogbook.Web.Areas.Manager.Models;


namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LogbooksController : Controller
    {
        private readonly ILogbookService logbookService;

        public LogbooksController(ILogbookService logbookService)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}