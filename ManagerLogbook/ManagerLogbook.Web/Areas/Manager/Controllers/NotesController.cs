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
using ManagerLogbook.Services.CustomExeptions;
using log4net;

namespace ManagerLogbook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class NotesController : Controller
    {
        private readonly IImageOptimizer optimizer;
        private readonly INoteService noteService;
        private readonly IUserService userService;
        private readonly ILogbookService logbookService;
        private readonly IMemoryCache cache;
        private static readonly ILog log =
        LogManager.GetLogger(typeof(NotesController));

        public NotesController(IImageOptimizer optimizer, INoteService noteService,
                              IUserService userService, ILogbookService logbookService, IMemoryCache cache)
        {
            this.optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
            this.noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [TempData] public string StatusMessage { get; set; }


        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new IndexNoteViewModel();

                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);

                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesAsync(userId, user.CurrentLogbookId.Value);
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                var logbooks = await this.logbookService.GetAllLogbooksByUserAsync(userId);

                var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);  

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

                model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
                model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();

                return View(model);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                StatusMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> NotesForDaysBefore(int id)
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);
                var model = new IndexNoteViewModel();
                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value, id);
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                //model.SearchModel = new SearchViewModel()
                //{
                //    Notes = notes
                //};
                var searchModel = new SearchViewModel();
                searchModel.Notes = notes;
                //model.Notes = notes;
                //model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
                //model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();
                return PartialView("_NoteListPartial", searchModel);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> ActiveNotes()
        {
            try
            {
                var userId = this.User.GetId();
                var user = await this.userService.GetUserByIdAsync(userId);
                var model = new IndexNoteViewModel();
                var logbookId = user.CurrentLogbookId;
                if (!user.CurrentLogbookId.HasValue)
                {
                    return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
                }
                var notesDTO = await this.noteService.ShowLogbookNotesWithActiveStatusAsync(userId, user.CurrentLogbookId.Value);
                var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                foreach (var note in notes)
                {
                    note.CanUserEdit = note.UserId == userId;
                }
                model.SearchModel = new SearchViewModel()
                {
                    Notes = notes
                };
                //model.Notes = notes;
                model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
                model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();
                return View(model);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
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
                    imageName = optimizer.OptimizeImage(model.NoteImage, 350, 235);
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
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
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
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var noteDTO = await this.noteService.GetNoteByIdAsync(id);

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

            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetNotesInPage(SearchViewModel model)
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserByIdAsync(userId);
            var currPage = model.CurrPage;

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this.noteService
                                     .SearchNotesAsync(userId, user.CurrentLogbookId.Value,
                                                                          model.StartDate, model.EndDate, model.CategoryId, model.SearchCriteria);

            var notes = notesDTO.Select(x => x.MapFrom()).ToList();

            foreach (var note in notes)
            {
                note.CanUserEdit = note.UserId == userId;
            }

            var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);

            //model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
            //model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();

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

            searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();

            return PartialView("_NoteListPartial", searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(IndexNoteViewModel model)
        {
            var userId = this.User.GetId();
            var user = await this.userService.GetUserByIdAsync(userId);
            
            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }
            var notesDTO = await this.noteService
                                     .SearchNotesAsync(userId, user.CurrentLogbookId.Value,
                                                                          model.StartDate, model.EndDate, model.CategoryId, model.SearchCriteria);
            var notes = notesDTO.Select(x => x.MapFrom()).ToList();
            foreach (var note in notes)
            {
                note.CanUserEdit = note.UserId == userId;
            }

            var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);


            //model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
            //model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();

            var searchModel = new SearchViewModel();

            searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();

            return PartialView("_NoteListPartial", searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchNotesScrollResult(SearchViewModel model)
        {
            if (model.ScrollPage == 0)
            {
                model.ScrollPage = 1;
            }
            var userId = this.User.GetId();
            var user = await this.userService.GetUserByIdAsync(userId);
            var currPage = model.CurrPage;

            var logbookId = user.CurrentLogbookId;

            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var notesDTO = await this.noteService
                                     .SearchNotesAsync(userId, user.CurrentLogbookId.Value,
                                                                          model.StartDate, model.EndDate, model.CategoryId, model.SearchCriteria, model.ScrollPage);

            var notes = notesDTO.Select(x => x.MapFrom()).ToList();

            foreach (var note in notes)
            {
                note.CanUserEdit = note.UserId == userId;
            }

            var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);

            //model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
            //model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();

            var searchModel = new SearchViewModel()
            {
                ScrollPage = model.ScrollPage + 1,
                TotalPages = totalPages
            };

            searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();

            return PartialView("_ScrollNotesListResultPartial", searchModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ScrollSearchResult(SearchViewModel model)
        //{
        //    var userId = this.User.GetId();
        //    var user = await this.userService.GetUserByIdAsync(userId);

        //    var logbookId = user.CurrentLogbookId;
        //    if (!user.CurrentLogbookId.HasValue)
        //    {
        //        return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
        //    }
        //    var notesDTO = await this.noteService
        //                             .SearchNotesAsync(userId, user.CurrentLogbookId.Value,
        //                                                                  model.StartDate, model.EndDate, model.CategoryId, model.SearchCriteria);
        //    var notes = notesDTO.Select(x => x.MapFrom()).ToList();
        //    foreach (var note in notes)
        //    {
        //        note.CanUserEdit = note.UserId == userId;
        //    }

        //    var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);


        //    //model.Categories = (await CacheNoteCategories()).Select(x => x.MapFrom()).ToList();
        //    //model.Logbooks = (await CacheLogbooks(userId)).Select(x => x.MapFrom()).ToList();

        //    var searchModel = new SearchViewModel();

        //    searchModel.Notes = notesDTO.Select(x => x.MapFrom()).ToList();

        //    return PartialView("_NoteListPartial", searchModel);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetFifteenNotesById(int? currPage)
        //{
        //    try
        //    {
        //        //Authorize

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }

        //        var userId = this.User.GetId();
        //        var user = await this.userService.GetUserByIdAsync(userId);

        //        var logbookId = user.CurrentLogbookId;
        //        if (!user.CurrentLogbookId.HasValue)
        //        {
        //            return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
        //        }

        //        var currentPage = currPage ?? 1;

        //        var totalPages = await this.noteService.GetPageCountForNotesAsync(15, user.CurrentLogbookId.Value);
        //        var fifteenNotesById = await noteService.Get15NotesByIdAsync(currentPage, user.CurrentLogbookId.Value);
        //        var result = fifteenNotesById.Select(x => x.MapFrom()).ToList();


        //        //var viewModel = new UserListViewModel()
        //        //{
        //        //    CurrPage = currentPage,
        //        //    TotalPages = totalPages,
        //        //    FiveUsersById = result
        //        //};

        //        if (totalPages > currentPage)
        //        {
        //            viewModel.NextPage = currentPage + 1;
        //        }

        //        if (currentPage > 1)
        //        {
        //            viewModel.PrevPage = currentPage - 1;
        //        }

        //        return PartialView("_UserListTable", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllNoteCategories()
        {
            try
            {
                var cacheCategories = await CacheNoteCategories();

                return Json(cacheCategories);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLogbooksByUser()
        {
            try
            {
                var userId = this.User.GetId();
                //var categories = await this.noteService.GetNoteCategoriesAsync();
                var cacheLogbooks = await CacheLogbooks(userId);

                return Json(cacheLogbooks);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception occured:", ex);
                return RedirectToAction("Error", "Home");
            }
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

        private async Task<IReadOnlyCollection<NoteGategoryDTO>> CacheNoteCategories()
        {
            var cashedCategories = await cache.GetOrCreateAsync<IReadOnlyCollection<NoteGategoryDTO>>("NoteCategories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.noteService.GetNoteCategoriesAsync();
            });
            return cashedCategories;
        }

        private async Task<ManagerViewModel> CreateDropdownLogbooks(ManagerViewModel model)
        {
            var cashedLogbooks = await CacheLogbooks(model.Id);

            model.Logbooks = cashedLogbooks.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<ManagerViewModel> EditDropdownLogbooks(ManagerViewModel model)
        {

            var cashedLogbooks = await CacheLogbooks(model.Id);

            List<SelectListItem> selectLogbooks = new List<SelectListItem>();

            foreach (var logbook in cashedLogbooks)
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

        private async Task<IReadOnlyCollection<LogbookDTO>> CacheLogbooks(string userId)
        {
            var cashedLogbooks = await cache.GetOrCreateAsync<IReadOnlyCollection<LogbookDTO>>("Logbooks", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.logbookService.GetAllLogbooksByUserAsync(userId);
            });

            return cashedLogbooks;
        }

    }
}