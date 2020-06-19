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

        public NoteService(ManagerLogbookContext context)
        {
            _context = context;
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId)
        {
            var note = new Note()
            {
                Description = model.Description,
                Image = model.Image,
                CreatedOn = DateTime.Now,
                NoteCategoryId = model.CategoryId,
                UserId = model.UserId,
                LogbookId = logbookId,
                IsActiveTask = model.IsActiveTask
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> DeactivateNoteActiveStatus(Note note, bool isActiveStatus)
        {
            note.IsActiveTask = isActiveStatus;
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> UpdateNoteAsync(NoteModel model, Note note)
        {
            SetNoteProperties(model, note);
            await _context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(int logbookId, int days)
        {
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

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(int logbookId)
        {
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

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(int logbookId)
        {
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

        public async Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, SearchNoteModel model)
        {
            IQueryable<Note> searchCollection;
            if (model.DaysBefore != null)
            {
                if (model.SearchCriteria != null)
                {
                    searchCollection = _context.Notes
                        .Include(x => x.NoteCategory)
                        .Include(x => x.User)
                        .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(model.SearchCriteria.ToLower().Replace(" ", string.Empty)) && mt.CreatedOn >= model.StartDate && mt.CreatedOn <= model.EndDate)
                        .OrderByDescending(x => x.CreatedOn);
                }
                else
                {
                    searchCollection = _context.Notes
                         .Include(x => x.NoteCategory)
                         .Include(x => x.User)
                         .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= model.StartDate && mt.CreatedOn <= model.EndDate)
                         .OrderByDescending(x => x.CreatedOn);
                }

            }
            else
            {
                if (model.SearchCriteria != null)
                {
                    searchCollection = _context.Notes
                        .Include(x => x.NoteCategory)
                        .Include(x => x.User)
                        .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(model.SearchCriteria.ToLower().Replace(" ", string.Empty)) && mt.CreatedOn >= model.StartDate && mt.CreatedOn <= model.EndDate)
                        .OrderByDescending(x => x.CreatedOn);
                }
                else
                {
                    searchCollection = _context.Notes
                         .Include(x => x.NoteCategory)
                         .Include(x => x.User)
                         .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= model.StartDate && mt.CreatedOn <= model.EndDate)
                         .OrderByDescending(x => x.CreatedOn);
                }
            }

            if (model.CategoryId.HasValue)
            {
                searchCollection = searchCollection.Where(x => x.NoteCategoryId == model.CategoryId);
            }

            var searchResult = await searchCollection.Select(x => x.ToDTO())
                .Skip((model.CurrPage - 1) * 15)
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

        public async Task<Note> GetNoteAsync(int id)
        {
            var note = await _context.Notes.SingleOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NoteNotFound);
            }

            return note;
        }

        public async Task<NoteCategory> GetNoteCategoryAsync(int id)
        {
            var noteCategory = await _context.NoteCategories.SingleOrDefaultAsync(x => x.Id == id);

            if (noteCategory == null)
            {
                throw new NotFoundException(ServicesConstants.NoteCategoryDoesNotExists);
            }

            return noteCategory;
        }

        public async Task<bool> CheckIfUserIsAuthorized(string userId, int logbookId)
        {
            var isUserAuthorized = await _context.UsersLogbooks.AnyAsync(x => x.UserId == userId && x.LogbookId == logbookId);

            if (!isUserAuthorized)
            {
                throw new NotAuthorizedException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }

            return isUserAuthorized;
        }

        private void SetNoteProperties(NoteModel model, Note note)
        {
            note.Description = model.Description;
            if (model.Image != null)
            {
                note.Image = model.Image;
            }

            if (model.CategoryId.HasValue)
            {
                note.NoteCategoryId = model.CategoryId;
                note.IsActiveTask = model.IsActiveTask;
            }
        }
    }
}

