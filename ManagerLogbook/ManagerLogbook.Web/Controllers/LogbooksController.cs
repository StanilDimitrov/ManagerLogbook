using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class LogbooksController : Controller
    {
        private readonly ILogbookService logbookService;
        private readonly IUserService userService;
        private readonly INoteService noteService;
        private readonly IUserServiceWrapper wrapper;

        public LogbooksController(ILogbookService logbookService,
                                  IUserService userService,
                                  INoteService noteService,
                                  IUserServiceWrapper wrapper)
        {
            this.logbookService = logbookService;
            this.userService = userService;
            this.noteService = noteService;
            this.wrapper = wrapper;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var logbook = await this.logbookService.GetLogbookDetailsAsync(id);
            var managers = await this.userService.GetAllManagersPresentInLogbookAsync(logbook.Id);

            var viewModel = new IndexLogbookViewModel
            {
                Logbook = logbook.MapFrom(),
                AssignedManagers = managers.Select(x => x.MapFrom()).ToList()
            };

            var userId = this.wrapper.GetLoggedUserId(User);
            if (User.IsInRole("Manager"))
            {
                viewModel.ActiveNotes = (await this.noteService.ShowLogbookNotesWithActiveStatusAsync(userId, logbook.Id)).Select(x => x.MapFrom()).ToList();
                viewModel.TotalNotes = (await this.noteService.ShowLogbookNotesAsync(userId, logbook.Id)).Select(x => x.MapFrom()).ToList();
            }

            return View(viewModel);
        }
    }
}