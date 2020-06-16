﻿using log4net;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Extensions;
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
        private readonly IImageOptimizer optimizer;
        private readonly INoteService noteService;
        private readonly IUserService userService;
        private readonly ILogbookService logbookService;
        private readonly IMemoryCache cache;
        private readonly IUserServiceWrapper wrapper;

        private static readonly ILog log =
        LogManager.GetLogger(typeof(NotesController));

        public NotesController(IImageOptimizer optimizer, 
                              IUserService userService, 
                              INoteService noteService,
                              ILogbookService logbookService, 
                              IMemoryCache cache,
                              IUserServiceWrapper wrapper)
                            
        {
            this.optimizer = optimizer;
            this.noteService = noteService;
            this.userService = userService;
            this.logbookService = logbookService;
            this.cache = cache;
            this.wrapper = wrapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new IndexNoteViewModel();

                var userId = this.wrapper.GetLoggedUserId(User);
                var user = await this.userService.GetUserDtoByIdAsync(userId);
                var logbooks = await this.logbookService.GetLogbooksByUserAsync(userId);
                var logbookId = user.CurrentLogbookId;
                
                if (user.CurrentLogbookId.HasValue)
                {
                    var currentLogbook = await this.logbookService.GetLogbookById(user.CurrentLogbookId.Value);
                    model.CurrentLogbook = new LogbookViewModel()
                    {
                        CurrentLogbookId = logbookId,
                        Name =currentLogbook.Name
                    };
                    model.Manager = new UserViewModel()
                    {
                        CurrentLogbookId = logbookId,
                        Id = userId
                    };

                    model.CurrentLogbookId = logbookId;
                   
                    var notesDTO = await this.noteService.Get15NotesByIdAsync(1, user.CurrentLogbookId.Value);

                    var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                   

                    foreach (var note in notes)
                    {
                        note.CanUserEdit = note.UserId == userId;
                    }

                    int notesPerPage = 15;
                    var totalPages = await noteService.GetPageCountForNotesAsync(notesPerPage, (int)logbookId);

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
                    model.Logbooks =  logbooks.Select(x => x.MapFrom()).ToList();
                    return View(model);
                }
                model.SearchModel = new SearchViewModel();
                model.Manager = user.MapFrom();
                
                return View(model);
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

        [HttpPost]
        public async Task<IActionResult> NotesForDaysBefore(int id)
        {
            try
            {
                var userId = this.wrapper.GetLoggedUserId(User);
                var user = await this.userService.GetUserDtoByIdAsync(userId);
                var model = new IndexNoteViewModel();
                var logbookId = user.CurrentLogbookId;
                if (user.CurrentLogbookId.HasValue)
                {
                    var notesDTO = await this.noteService.ShowLogbookNotesForDaysBeforeAsync(userId, user.CurrentLogbookId.Value, id);
                    var notes = notesDTO.Select(x => x.MapFrom()).ToList();
                    foreach (var note in notes)
                    {
                        note.CanUserEdit = note.UserId == userId;
                    }
                   
                    var searchModel = new SearchViewModel();
                    searchModel.Notes = notes;
                    return PartialView("_NoteListPartial", searchModel);
                }

                return BadRequest(WebConstants.NoLogbookChoosen);

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
        public async Task<IActionResult> ActiveNotes()
        {
            try
            {
                var userId = this.wrapper.GetLoggedUserId(User);
                var user = await this.userService.GetUserDtoByIdAsync(userId);
                var logbookId = user.CurrentLogbookId;
                var searchModel = new SearchViewModel();

                if (user.CurrentLogbookId.HasValue)
                {
                    var notesDTO = await this.noteService.ShowLogbookNotesWithActiveStatusAsync(userId, user.CurrentLogbookId.Value);
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
        public async Task<IActionResult> Create(NoteViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(WebConstants.EnterValidData);
            }

            try
            {
                string imageName = null;

                if (model.NoteImage != null)
                {
                    imageName = optimizer.OptimizeImage(model.NoteImage, 600, 800);
                }
                var userId = this.wrapper.GetLoggedUserId(User);
                
                var user = await this.userService.GetUserDtoByIdAsync(userId);

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

                return BadRequest(WebConstants.UnableToCreateNote);
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
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            
            try
            {
                string imageName = null;

                if (model.NoteImage != null)
                {
                    imageName = optimizer.OptimizeImage(model.NoteImage, 600, 800);
                }

                if (model.Image != null)
                {
                    optimizer.DeleteOldImage(model.Image);
                }

                var userId = this.wrapper.GetLoggedUserId(User);
                var noteDTO = await this.noteService
                                  .EditNoteAsync(model.Id, userId, model.Description,
                                                 imageName, model.CategoryId);

                if (noteDTO.Description != model.Description)
                {
                    return BadRequest(string.Format(WebConstants.UnableToEditNote));
                }

                return Ok(string.Format(WebConstants.NoteEdited));
                
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

        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var noteDTO = await this.noteService.GetNoteByIdAsync(id);

                var userId = this.wrapper.GetLoggedUserId(User);
                var user = await this.userService.GetUserDtoByIdAsync(userId);
                var logbookId = user.CurrentLogbookId;
                if (user.CurrentLogbookId.HasValue)
                {
                    noteDTO = await this.noteService
                              .DeactivateNoteActiveStatus(noteDTO.Id, userId);
                    if (noteDTO.IsActiveTask)
                    {
                        return BadRequest(string.Format(WebConstants.UnableToDisableStatusNote));
                    }

                    var notesDTO = await this.noteService.ShowLogbookNotesAsync(userId, user.CurrentLogbookId.Value);
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
        public async Task<IActionResult> SearchNotesInPage(SearchViewModel model)
        {
            var userId = this.wrapper.GetLoggedUserId(User);
            var user = await this.userService.GetUserDtoByIdAsync(userId);

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

            var notesDTO = await this.noteService
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
           
            var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);

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
            var userId = this.wrapper.GetLoggedUserId(User);
            var user = await this.userService.GetUserDtoByIdAsync(userId);

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

            var notesDTO = await this.noteService
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
                totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId);
            }
            else
            {
                totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);
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
            var userId = this.wrapper.GetLoggedUserId(User);
            var user = await this.userService.GetUserDtoByIdAsync(userId);
            
            var logbookId = user.CurrentLogbookId;
            if (!user.CurrentLogbookId.HasValue)
            {
                return BadRequest(string.Format(WebConstants.NoLogbookChoosen));
            }

            var totalPages = await noteService.GetPageCountForNotesAsync(15, (int)logbookId, model.SearchCriteria);


            if (totalPages < model.CurrPage)
            {
                model.CurrPage = 1;
            }

            var notesDTO = await this.noteService
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
            try
            {
                var cacheCategories = await CacheNoteCategories();

                return Json(cacheCategories);
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
                var userId = this.wrapper.GetLoggedUserId(User);
                var logbooks = await this.logbookService.GetLogbooksByUserAsync(userId);

                return Json(logbooks);
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
            var cashedCategories = await cache.GetOrCreateAsync<IReadOnlyCollection<NoteGategoryDTO>>("NoteCategories", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                return await this.noteService.GetNoteCategoriesAsync();
            });
            return cashedCategories;
        }

        private async Task<UserViewModel> CreateDropdownLogbooks(UserViewModel model)
        {
            var logbooks = await this.logbookService.GetLogbooksByUserAsync(model.Id);

            model.Logbooks = logbooks.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            return model;
        }

        private async Task<UserViewModel> EditDropdownLogbooks(UserViewModel model)
        {

            var logbooks = await this.logbookService.GetLogbooksByUserAsync(model.Id);

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