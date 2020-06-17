using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class NotesController : Controller
    {
        private readonly IImageOptimizer _optimizer;
        private readonly INoteService _noteService;
        private readonly IUserService _userService;
        private readonly ILogbookService _logbookService;
        private readonly IMemoryCache _cache;
        private readonly IUserServiceWrapper _wrapper;

        public NotesController(IImageOptimizer optimizer,
                              IUserService userService,
                              INoteService noteService,
                              ILogbookService logbookService,
                              IMemoryCache cache,
                              IUserServiceWrapper wrapper)

        {
            _optimizer = optimizer;
            _noteService = noteService;
            _userService = userService;
            _logbookService = logbookService;
            _cache = cache;
            _wrapper = wrapper;
        }

        public async Task<IActionResult> Index()
        {

            var model = new IndexNoteViewModel();

            var userId = this._wrapper.GetLoggedUserId(User);
            var user = await this._userService.GetUserDtoByIdAsync(userId);
            var logbooks = await this._logbookService.GetLogbooksByUserAsync(userId);
            var logbookId = user.CurrentLogbookId;

            if (user.CurrentLogbookId.HasValue)
            {
                var currentLogbook = await this._logbookService.GetLogbookDetailsAsync(user.CurrentLogbookId.Value);
                model.CurrentLogbook = new LogbookViewModel()
                {
                    CurrentLogbookId = logbookId,
                    Name = currentLogbook.Name
                };
                model.Manager = new UserViewModel()
                {
                    CurrentLogbookId = logbookId,
                    Id = userId
                };

                model.CurrentLogbookId = logbookId;

                var notesDTO = await this._noteService.Get15NotesByIdAsync(1, user.CurrentLogbookId.Value);

                var notes = notesDTO.Select(x => x.MapFrom()).ToList();

                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }

                int notesPerPage = 15;
                var totalPages = await _noteService.GetPageCountForNotesAsync(notesPerPage, (int)logbookId);

                model.SearchModel = new SearchViewModel()
                {
                    CurrPage = 1,
                    TotalPages = totalPages,
                    Notes = notes
                };

                if (totalPages > 1)
                {
                    model.SearchModel.NextPage = 2;
                }

                model.SearchModel.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
                model.Logbooks = logbooks.Select(x => x.MapFrom()).ToList();
                return View(model);
            }
            model.SearchModel = new SearchViewModel();
            model.Manager = user.MapFrom();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NotesForDaysBefore(int id)
        {

            var userId = this._wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoByIdAsync(userId);
            var model = new IndexNoteViewModel();
            var logbookId = user.CurrentLogbookId;
            if (user.CurrentLogbookId.HasValue)
            {
                var notesDTO = await _noteService.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value, id);
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }

                var searchModel = new SearchViewModel
                {
                    Notes = notes
                };

                return PartialView("_NoteListPartial", searchModel);
            }

            return BadRequest(WebConstants.NoLogbookChoosen);
        }

        [HttpGet]
        public async Task<IActionResult> ActiveNotes()
        {
            var userId = this._wrapper.GetLoggedUserId(User);
            var user = await this._userService.GetUserDtoByIdAsync(userId);
            var logbookId = user.CurrentLogbookId;
            var searchModel = new SearchViewModel();

            if (user.CurrentLogbookId.HasValue)
            {
                var notesDTO = await this._noteService.ShowLogbookNotesWithActiveStatusAsync(userId, user.CurrentLogbookId.Value);
                if (notesDTO != null)
                {
                    var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                    foreach (var note in notes)
                    {
                        note.CanUserEdit = note.UserId == userId;
                    }
                    searchModel.Notes = notes;
                }
                searchModel.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
                return PartialView("_NoteListPartial", searchModel);
            }

            return BadRequest(WebConstants.NoLogbookChoosen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            string imageName = null;

            if (viewModel.NoteImage != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.NoteImage, 600, 800);
            }
            var userId = _wrapper.GetLoggedUserId(User);

            var user = await _userService.GetUserDtoByIdAsync(userId);

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }
            var model = viewModel.MapFrom();
            model.Image = imageName;

            var note = await _noteService.CreateNoteAsync(model, user.CurrentLogbookId.Value, userId);

            if (note.Description == viewModel.Description)
            {
                return Ok(string.Format(WebConstants.NoteCreated));
            }

            return BadRequest(WebConstants.UnableToCreateNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteViewModel viewModel)
        {
            string imageName = null;

            if (viewModel.NoteImage != null)
            {
                imageName = _optimizer.OptimizeImage(viewModel.NoteImage, 600, 800);
            }

            if (viewModel.Image != null)
            {
                _optimizer.DeleteOldImage(viewModel.Image);
            }

            var userId = this._wrapper.GetLoggedUserId(User);
            var model = viewModel.MapFrom();
            model.Image = imageName;

            var noteDTO = await this._noteService.EditNoteAsync(model, userId);

            if (noteDTO.Description != viewModel.Description)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditNote));
            }

            return Ok(string.Format(WebConstants.NoteEdited));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoByIdAsync(userId);
            var logbookId = user.CurrentLogbookId;
            if (user.CurrentLogbookId.HasValue)
            {
                var noteDTO = await _noteService.DeactivateNoteActiveStatus(id, userId);

                if (noteDTO.IsActiveTask)
                {
                    return BadRequest(string.Format(WebConstants.UnableToDisableStatusNote));
                }

                var notesDTO = await _noteService.ShowLogbookNotesAsync(userId, user.CurrentLogbookId.Value);
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                var searchModel = new SearchViewModel();
                searchModel.Notes = notes;
                return PartialView("_NoteListPartial", searchModel);
            }

            return BadRequest(WebConstants.UnableToDisableStatusNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchNotesInPage(SearchViewModel model)
        {
            var userId = this._wrapper.GetLoggedUserId(User);
            var user = await this._userService.GetUserDtoByIdAsync(userId);

            var currPage = 1;

            if (model.CurrPage > 0)
            {
                currPage = model.CurrPage;
            }

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this._noteService
                                     .SearchNotesAsync(
                userId,
                user.CurrentLogbookId.Value,
                model.StartDate,
                model.EndDate,
                model.CategoryId,
                model.SearchCriteria,
                int.MaxValue,
                model.CurrPage);

            if (notesDTO != null)
            {
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
            }

            var totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);

            var searchModel = new SearchViewModel()
            {
                CurrPage = model.CurrPage,
                TotalPages = totalPages
            };

            if (totalPages > currPage)
            {
                searchModel.NextPage = currPage + 1;
            }

            if (currPage > 1)
            {
                searchModel.PrevPage = currPage - 1;
            }
            if (notesDTO != null)
            {
                searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();
            }

            return PartialView("_NoteListPartial", searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetNotesInPage(SearchViewModel model)
        {
            var userId = this._wrapper.GetLoggedUserId(User);
            var user = await this._userService.GetUserDtoByIdAsync(userId);

            var currPage = 1;

            if (model.CurrPage > 0)
            {
                currPage = model.CurrPage;
            }

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this._noteService
                                     .SearchNotesAsync(
                userId,
                user.CurrentLogbookId.Value,
                model.StartDate,
                model.EndDate,
                model.CategoryId,
                model.SearchCriteria,
                int.MaxValue,
                model.CurrPage);

            if (notesDTO != null)
            {
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();

                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
            }

            var totalPages = 1;

            if (string.IsNullOrEmpty(model.SearchCriteria))
            {
                totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId);
            }
            else
            {
                totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);
            }

            var searchModel = new SearchViewModel()
            {
                CurrPage = model.CurrPage,
                TotalPages = totalPages
            };

            if (notesDTO != null)
            {
                searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();
            }

            if (totalPages > currPage)
            {
                searchModel.NextPage = currPage + 1;
            }

            if (currPage > 1)
            {
                searchModel.PrevPage = currPage - 1;
            }

            return PartialView("_NoteListPartial", searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoByIdAsync(userId);

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);


            if (totalPages < model.CurrPage)
            {
                model.CurrPage = 1;
            }

            var notesDTO = await this._noteService
                                     .SearchNotesAsync(userId, user.CurrentLogbookId.Value,
                                                      model.StartDate, model.EndDate, model.CategoryId,
                                                      model.SearchCriteria, model.DaysBefore, model.CurrPage);
            if (notesDTO != null)
            {
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }

                model.Notes = notesDTO.Select(x => x.MapFrom()).ToList();
            }

            if (totalPages > model.CurrPage)
            {
                model.NextPage = model.CurrPage + 1;
            }

            if (model.CurrPage > 1)
            {
                model.PrevPage = model.CurrPage - 1;
                model.TotalPages = totalPages;
            }

            return PartialView("_NoteListPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoteCategories()
        {
            var cacheCategories = await CacheNoteCategories();

            return Json(cacheCategories);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogbooksByUser()
        {
            var userId = this._wrapper.GetLoggedUserId(User);
            var logbooks = await this._logbookService.GetLogbooksByUserAsync(userId);

            return Json(logbooks);
        }

        private async Task<NoteViewModel> CreateDropdownNoteCategories(NoteViewModel model)
        {
            var cashedCategories = await CacheNoteCategories();

            model.Categories = cashedCategories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<NoteViewModel> EditDropdownNoteCategories(NoteViewModel model)
        {

            var cashedCategories = await CacheNoteCategories();

            List<SelectListItem> selectCategories = new List<SelectListItem>();
            if (cashedCategories != null)
            {
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
            }

            model.Categories = selectCategories;

            return model;
        }

        private async Task<IReadOnlyCollection<NoteGategoryDTO>> CacheNoteCategories()
        {
            var cashedCategories = await _cache.GetOrCreateAsync<IReadOnlyCollection<NoteGategoryDTO>>("NoteCategories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this._noteService.GetNoteCategoriesAsync();
            });
            return cashedCategories;
        }

        private async Task<UserViewModel> CreateDropdownLogbooks(UserViewModel model)
        {
            var logbooks = await this._logbookService.GetLogbooksByUserAsync(model.Id);

            model.Logbooks = logbooks.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<UserViewModel> EditDropdownLogbooks(UserViewModel model)
        {

            var logbooks = await this._logbookService.GetLogbooksByUserAsync(model.Id);

            List<SelectListItem> selectLogbooks = new List<SelectListItem>();

            foreach (var logbook in logbooks)
            {
                if (logbook.Name == model.LogbookName)
                {
                    selectLogbooks.Add(new SelectListItem(logbook.Name, logbook.Id.ToString(), true));
                }
                else
                {
                    selectLogbooks.Add(new SelectListItem(logbook.Name, logbook.Id.ToString()));
                }
            }

            model.Logbooks = selectLogbooks;
            return model;
        }

    }
}