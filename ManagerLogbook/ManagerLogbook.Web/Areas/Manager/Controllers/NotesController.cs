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

namespace ManagerLogbook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class NotesController : Controller
    {
        private readonly IImageOptimizer optimizer;
        private readonly INoteService noteService;
        private readonly IUserService userService;
        private readonly IMemoryCache cache;

        public NotesController(IImageOptimizer optimizer, INoteService noteService,
                              IUserService userService, IMemoryCache cache)
        {
            this.optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
            this.noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [TempData] public string StatusMessage { get; set; }


         public async Task<IActionResult> NotesForDaysBefore(int id)
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value,id);
                var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach(var note in noteViewModel)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                return View(noteViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesAsync(userId, user.CurrentLogbookId.Value);
                var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach(var note in noteViewModel)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                return View(noteViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> ActiveNotes()
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesWithActiveStatusAsync(userId, user.CurrentLogbookId.Value);
                var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in noteViewModel)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
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
                var user = await this.userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }

                var note = await this.noteService.CreateNoteAsync(userId, user.CurrentLogbookId.Value,
                                                                  model.Description, imageName, model.CategoryId);
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditNote));
                //return View(model);
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
                                                 imageName, model.CategoryId);

                if (noteDTO.Description != model.Description)
                {
                    return BadRequest(string.Format(WebConstants.UnableToEditNote));
                }

                return Ok(string.Format(WebConstants.NoteEdited));
                //return RedirectToAction("Index");

            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id)
        {
            try
            {
                var noteDTO = await this.noteService.GetNoteByIdAsync(id);
                if (noteDTO == null)
                {
                    return NotFound();
                }

                string imageName = null;

                if (noteDTO.NoteImage != null)
                {
                    imageName = optimizer.OptimizeImage(noteDTO.NoteImage, 268, 182);
                }

                if (noteDTO.Image != null)
                {
                    optimizer.DeleteOldImage(noteDTO.Image);
                }

                var userId = this.User.GetId();
                noteDTO = await this.noteService
                                    .EditNoteAsync(noteDTO.Id, userId, noteDTO.Description,
                                                 noteDTO.Image, noteDTO.CategoryId);

                return Ok(string.Format(WebConstants.NoteEdited));
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var noteDTO = await this.noteService.GetNoteByIdAsync(id);
                if (noteDTO == null)
                {
                    return NotFound();
                }

                var userId = this.User.GetId();
                noteDTO = await this.noteService
                                  .DeactivateNoteActiveStatus(noteDTO.Id, userId);

                if (noteDTO.IsActiveTask)
                {
                    return RedirectToAction("Index");
                    //return BadRequest(string.Format(WebConstants.UnableToDisableStatusNote));
                }

                //return Ok(string.Format(WebConstants.SuccessfullyDeactivateActiveStatus));
                return RedirectToAction("Index");
            }

            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        
        public async Task<IActionResult> Search(NoteViewModel model, string searchCriteria)
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }
            var notesDTO = await this.noteService
                                   .SearchNotesByDateAndStringStringAsync(userId, user.CurrentLogbookId.Value,
                                                                          model.StartDate, model.EndDate, searchCriteria);
            var noteViewModel = notesDTO.Select(x => x.MapFrom()).ToList();
            return View(noteViewModel);
        }

        private async Task<NoteViewModel> CreateDropdown(NoteViewModel model)
        {
            var cashedCategories = await CacheCategories();
           
            model.Categories = cashedCategories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
           
            return model;
        }

        private async Task<NoteViewModel> EditDropdown(NoteViewModel model)
        {

            var cashedCategories = await CacheCategories();
            
            List<SelectListItem> selectCategories = new List<SelectListItem>();

            foreach (var category in cashedCategories)
            {
                if (category.Name == model.CategoryName)
                {
                    selectCategories.Add(new SelectListItem(category.Name, category.Id.ToString(), true));
                }
                else
                {
                    selectCategories.Add(new SelectListItem(category.Name, category.Id.ToString()));
                }
            }

            model.Categories = selectCategories;
            
            return model;
        }

        private async Task<IReadOnlyCollection<NoteGategoryDTO>> CacheCategories()
        {
            var cashedCategories = await cache.GetOrCreateAsync<IReadOnlyCollection<NoteGategoryDTO>>("Categories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.noteService.GetNoteCategoriesAsync();
            });

            return cashedCategories;
        }

        
    }
}