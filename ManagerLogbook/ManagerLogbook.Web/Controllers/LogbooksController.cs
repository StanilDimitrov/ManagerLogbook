using System;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class LogbooksController : Controller
    {
        private readonly ILogbookService logbookService;
        private readonly IUserService userService;
        private readonly INoteService noteService;

        public LogbooksController(ILogbookService logbookService,
                                  IUserService userService,
                                  INoteService noteService)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new IndexLogbookViewModel();

            var logbook = await this.logbookService.GetLogbookById(id);

            model.Logbook = logbook.MapFrom();

            model.AssignedManagers = await this.userService.GetAllManagersPresentInLogbookAsync(logbook.Id);

            var userId = this.User.GetId();

            if (User.IsInRole("Manager"))
            {
                model.ActiveNotes = await this.noteService.ShowLogbookNotesWithActiveStatusAsync(userId, logbook.Id);
                model.TotalNotes = await this.noteService.ShowLogbookNotesAsync(userId, logbook.Id);
            }

            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}