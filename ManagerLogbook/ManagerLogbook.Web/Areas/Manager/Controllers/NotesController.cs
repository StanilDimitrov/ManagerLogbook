using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Mappers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly INoteEngine _noteEngine;
        private readonly IUserService _userService;
        private readonly ILogbookService _logbookService;
        private readonly IMemoryCache _cache;
        private readonly IUserServiceWrapper _wrapper;

        public NotesController(IImageOptimizer optimizer,
                              IUserService userService,
                              INoteService noteService,
                              INoteEngine noteEngine,
                              ILogbookService logbookService,
                              IMemoryCache cache,
                              IUserServiceWrapper wrapper)

        {
            _optimizer = optimizer;
            _noteService = noteService;
            _noteEngine = noteEngine;
            _userService = userService;
            _logbookService = logbookService;
            _cache = cache;
            _wrapper = wrapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexNoteViewModel();

            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);
            var logbooks = await _logbookService.GetLogbooksByUserAsync(userId);
            var logbookId = user.CurrentLogbookId;

            if (user.CurrentLogbookId.HasValue)
            {
                var currentLogbook = await _logbookService.GetLogbookDetailsAsync(user.CurrentLogbookId.Value);
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

                var notesDTO = await _noteService.Get15NotesByIdAsync(1, user.CurrentLogbookId.Value);

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
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);
            var model = new IndexNoteViewModel();
            var logbookId = user.CurrentLogbookId;
            if (user.CurrentLogbookId.HasValue)
            {
                var notesDTO = await _noteEngine.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value, id);
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
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);
            var logbookId = user.CurrentLogbookId;
            var searchModel = new SearchViewModel();

            if (user.CurrentLogbookId.HasValue)
            {
                var notesDTO = await _noteEngine.ShowLogbookNotesWithActiveStatusAsync(userId, user.CurrentLogbookId.Value);
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

            var user = await _userService.GetUserDtoAsync(userId);

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }
            var model = viewModel.MapFrom();
            model.Image = imageName;

            var note = await _noteEngine.CreateNoteAsync(model, user.CurrentLogbookId.Value, userId);

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

            var userId = _wrapper.GetLoggedUserId(User);
            var model = viewModel.MapFrom();
            model.Image = imageName;
            model.UserId = userId;

            var noteDTO = await _noteEngine.UpdateNoteAsync(model);

            if (noteDTO.Description != viewModel.Description)
            {
                return BadRequest(string.Format(WebConstants.UnableToEditNote));
            }

            return Ok(string.Format(WebConstants.NoteEdited));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);
            var logbookId = user.CurrentLogbookId;
            if (user.CurrentLogbookId.HasValue)
            {
                var noteDTO = await _noteEngine.DeactivateNoteActiveStatus(id, userId);

                if (noteDTO.IsActiveTask)
                {
                    return BadRequest(string.Format(WebConstants.UnableToDisableStatusNote));
                }

                var notesDTO = await _noteEngine.ShowLogbookNotesAsync(userId, user.CurrentLogbookId.Value);
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
        public async Task<IActionResult> SearchNotesInPage(SearchViewModel viewModel)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);

            var currPage = 1;

            if (viewModel.CurrPage > 0)
            {
                currPage = viewModel.CurrPage;
            }

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var model = viewModel.MapFrom();
            var notesDTO = await _noteEngine.SearchNotesAsync(userId, user.CurrentLogbookId.Value, model);


            if (notesDTO != null)
            {
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
            }

            var totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId, viewModel.SearchCriteria);

            var searchModel = new SearchViewModel()
            {
                CurrPage = viewModel.CurrPage,
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

            return PartialView("_NoteListPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetNotesInPage(SearchViewModel viewModel)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);

            var currPage = 1;

            if (viewModel.CurrPage > 0)
            {
                currPage = viewModel.CurrPage;
            }

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }
            var model = viewModel.MapFrom();
            var notesDTO = await _noteEngine.SearchNotesAsync(userId, user.CurrentLogbookId.Value, model);

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
        public async Task<IActionResult> Search(SearchViewModel viewModel)
        {
            var userId = _wrapper.GetLoggedUserId(User);
            var user = await _userService.GetUserDtoAsync(userId);

            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var totalPages = await _noteService.GetPageCountForNotesAsync(15, (int)logbookId, viewModel.SearchCriteria);


            if (totalPages < viewModel.CurrPage)
            {
                viewModel.CurrPage = 1;
            }

            var model = viewModel.MapFrom();

            var notesDTO = await _noteEngine.SearchNotesAsync(userId, user.CurrentLogbookId.Value, model);

            if (notesDTO != null)
            {
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }

                viewModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();
            }

            if (totalPages > viewModel.CurrPage)
            {
                viewModel.NextPage = viewModel.CurrPage + 1;
            }

            if (viewModel.CurrPage > 1)
            {
                viewModel.PrevPage = viewModel.CurrPage - 1;
                viewModel.TotalPages = totalPages;
            }

            return PartialView("_NoteListPartial", viewModel);
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
            var userId = _wrapper.GetLoggedUserId(User);
            var logbooks = await _logbookService.GetLogbooksByUserAsync(userId);

            return Json(logbooks);
        }

        private async Task<IReadOnlyCollection<NoteGategoryDTO>> CacheNoteCategories()
        {
            var cashedCategories = await _cache.GetOrCreateAsync<IReadOnlyCollection<NoteGategoryDTO>>("NoteCategories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await _noteService.GetNoteCategoriesAsync();
            });
            return cashedCategories;
        }
    }
}