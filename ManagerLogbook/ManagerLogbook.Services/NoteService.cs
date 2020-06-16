using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class NoteService : INoteService
    {
        private readonly ManagerLogbookContext _context;
        private readonly IUserService _userService;

        public NoteService(ManagerLogbookContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<NoteDTO> GetNoteDtoAsync(int id)
        {
            var note = await _context.Notes
                             .Include(x => x.User)
                             .Include(x => x.NoteCategory)
                             .FirstOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NotNotFound);
            }

            return note.ToDTO();
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId)
        {
            CheckIfUserIsAuthorized(userId, logbookId);

            var note = new Note()
            {
                Description = model.Description,
                Image = model.Image,
                CreatedOn = DateTime.Now,
                NoteCategoryId = model.CategoryId,
                UserId = model.UserId,
                LogbookId = logbookId
            };

            if (model.CategoryId.HasValue)
            {
                var noteCategory = await GetNoteCategoryAsync(model.CategoryId.Value);

                if (noteCategory.Name == "Task")
                {
                    note.IsActiveTask = true;
                }
            }

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId)
        {
            var note = await GetNoteAsync(noteId);
            var user = await _userService.GetUserAsync(userId);

            var logbookId = note.LogbookId;
            CheckIfUserIsAuthorized(user.Id, logbookId);

            note.IsActiveTask = false;
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> EditNoteAsync(NoteModel model, string userId)
        {
            var note = await GetNoteAsync(model.Id);
            var user = await _userService.GetUserAsync(model.UserId);

            if (note.UserId != model.UserId)
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote, user.UserName));
            }

            await SetNoteProperties(model, note);
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days)
        {
            CheckIfUserIsAuthorized(userId, logbookId);

            return await this._context.Notes
                           .Include(mt => mt.Logbook)
                           .Include(mt => mt.NoteCategory)
                           .Include(mt => mt.User)
                               .ThenInclude(lb => lb.UsersLogbooks)
                           .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn.Date >= DateTime.Now.Date.AddDays(-days))
                           .OrderByDescending(x => x.CreatedOn)
                           .Select(x => x.ToDTO())
                           .ToListAsync();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId)
        {
            CheckIfUserIsAuthorized(userId, logbookId);

            return await _context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.IsActiveTask)
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId)
        {
            CheckIfUserIsAuthorized(userId, logbookId);

            return await _context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId)
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();

        }
        public async Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(
            string userId,
            int logbookId,
            DateTime startDate,
            DateTime endDate,
            int? categoryId,
            string searchCriteria,
            int? daysBefore,
            int currPage = 1)
        {
            CheckIfUserIsAuthorized(userId, logbookId);

            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }

            IQueryable<Note> searchCollection;
            if (daysBefore != null)
            {
                if (searchCriteria != null)
                {
                    searchCollection = _context.Notes
                        .Include(x => x.NoteCategory)
                        .Include(x => x.User)
                        .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(searchCriteria.ToLower().Replace(" ", string.Empty)) && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
                        .OrderByDescending(x => x.CreatedOn);
                }
                else
                {
                    searchCollection = _context.Notes
                         .Include(x => x.NoteCategory)
                         .Include(x => x.User)
                         .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
                         .OrderByDescending(x => x.CreatedOn);
                }

            }
            else
            {
                if (searchCriteria != null)
                {
                    searchCollection = _context.Notes
                        .Include(x => x.NoteCategory)
                        .Include(x => x.User)
                        .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(searchCriteria.ToLower().Replace(" ", string.Empty)) && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
                        .OrderByDescending(x => x.CreatedOn);
                }
                else
                {
                    searchCollection = _context.Notes
                         .Include(x => x.NoteCategory)
                         .Include(x => x.User)
                         .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
                         .OrderByDescending(x => x.CreatedOn);
                }
            }

            if (categoryId.HasValue)
            {
                searchCollection = searchCollection.Where(x => x.NoteCategoryId == categoryId);
            }

            var searchResult = await searchCollection.Select(x => x.ToDTO())
                .Skip((currPage - 1) * 15)
                .Take(15).ToListAsync();

            return searchResult;
        }

        public async Task<IReadOnlyCollection<NoteGategoryDTO>> GetNoteCategoriesAsync()
        {
            return await _context.NoteCategories
                         .Select(x => x.ToDTO())
                          .ToListAsync();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> Get15NotesByIdAsync(int currPage, int logbookId)
        {
            if (currPage == 1)
            {
                return await _context.Notes.Include(x => x.NoteCategory)
                                          .Where(x => x.LogbookId == logbookId)
                                          .OrderByDescending(x => x.CreatedOn)
                                          .Take(15)
                                          .Select(x => x.ToDTO())
                                          .ToListAsync();
            }
            else
            {
                return await _context
                                    .Notes.Include(x => x.NoteCategory)
                                    .Where(x => x.LogbookId == logbookId)
                                    .OrderByDescending(x => x.CreatedOn)
                                    .Skip((currPage - 1) * 15)
                                    .Take(15)
                                    .Select(x => x.ToDTO())
                                    .ToListAsync();
            }
        }
        public async Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId)
        {
            var allNotesCount = await _context.Notes.Where(x => x.LogbookId == logbookId).CountAsync();

            int pageCount = (allNotesCount - 1) / notesPerPage + 1;

            return pageCount;
        }

        public async Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId, string searchPhrase)
        {
            var allNotesCount = await _context.Notes.Where(x => x.LogbookId == logbookId && x.Description.Contains(searchPhrase)).CountAsync();

            int pageCount = (allNotesCount - 1) / notesPerPage + 1;

            return pageCount;
        }

        private async Task<Note> GetNoteAsync(int id)
        {
            var note = await _context.Notes.SingleOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NotNotFound);
            }

            return note;
        }

        private async Task<NoteCategory> GetNoteCategoryAsync(int id)
        {
            var noteCategory = await this._context.NoteCategories.SingleOrDefaultAsync(x => x.Id == id);

            if (noteCategory == null)
            {
                throw new NotFoundException(ServicesConstants.NoteCategoryDoesNotExists);
            }

            return noteCategory;
        }

        private void CheckIfUserIsAuthorized(string userId, int logbookId)
        {
            if (!_context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }
        }

        private async Task SetNoteProperties(NoteModel model, Note note)
        {
            note.Description = model.Description;
            if (model.Image != null)
            {
                note.Image = model.Image;
            }

            if (model.CategoryId.HasValue)
            {
                note.NoteCategoryId = model.CategoryId;
                var noteCategory = await GetNoteCategoryAsync(model.CategoryId.Value);

                if (noteCategory.Name == "Task")
                {
                    note.IsActiveTask = true;
                }
            }
        }
    }
}

