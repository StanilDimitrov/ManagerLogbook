using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Extensions;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerLogbook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class NotesController : Controller
    {
        private readonly IImageOptimizer optimizer;
        private readonly INoteService noteService;
        private readonly IUserService userService;

        public NotesController(IImageOptimizer optimizer, INoteService noteService,
                              IUserService userService)
        {
            this.optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
            this.noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [TempData] public string StatusMessage { get; set; }



        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesPerDayAsync(userId, user.CurrentLogbookId.Value);
                var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
                return View(noteViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string imageName = null;

                if (model.NoteImage != null)
                {
                    imageName = optimizer.OptimizeImage(model.NoteImage, 268, 182);
                }

                var userId = this.User.GetId();
                var user = await this.userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }

                var note = await this.noteService.CreateNoteAsync(userId, user.CurrentLogbookId.Value,
                                                                  model.Description, model.Image, model.CategoryId);
                if (note.Description == model.Description)
                {
                    return Ok(string.Format(WebConstants.NoteCreated));
                }

                return RedirectToAction("Error", "Home");
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var noteDTO = await this.noteService.GetNoteByIdAsync(id);
            var noteViewModel = noteDTO.MapFrom();
            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var noteDTO = await this.noteService.GetNoteByIdAsync(model.Id);
                if (noteDTO == null)
                {
                    return NotFound();
                }

                string imageName = null;

                if (model.NoteImage != null)
                {
                    imageName = optimizer.OptimizeImage(model.NoteImage, 268, 182);
                }

                if (model.Image != null)
                {
                    optimizer.DeleteOldImage(model.Image);
                }

                var userId = this.User.GetId();
                noteDTO = await this.noteService
                                  .EditNoteAsync(noteDTO.Id, userId, model.Description,
                                                 model.Image, model.CategoryId);

                if (noteDTO.Description != model.Description)
                {
                    return BadRequest(string.Format(WebConstants.UnableToEditNote));
                }

                return Ok(string.Format(WebConstants.NoteEdited));
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }




        public async Task<IActionResult> NotesPerDay()
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this.noteService.ShowLogbookNotesPerDayAsync(userId, user.CurrentLogbookId.Value);
            var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
            return View(noteViewModel);
        }

        public async Task<IActionResult> NotesForDaysBefore(NoteViewModel model)
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this.noteService.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value, model.SearchPeriod);
            var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
            return View(noteViewModel);
        }

        public async Task<IActionResult> NotesForRangePeriod(NoteViewModel model)
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this.noteService.ShowLogbookNotesForDateRangeAsync(userId, user.CurrentLogbookId.Value,
                                                                                    model.StartDate, model.EndDate);
            var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
            return View(noteViewModel);
        }


    }
}