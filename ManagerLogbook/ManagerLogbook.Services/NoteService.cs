using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
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
        private readonly ManagerLogbookContext context;
        private readonly IBusinessValidator validator;

        public NoteService(ManagerLogbookContext context,
                            IBusinessValidator validator)

        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id)
        {
            var note = await this.context.Notes.FindAsync(id);

            return note.ToDTO();
        }

        public async Task<NoteDTO> CreateNoteAsync(string userId, int logbookId, string description, 
                                             string image, int? categoryId)
        {

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException(ServicesConstants.DescriptionCanNotBeNull);
            }

            validator.IsDescriptionInRange(description);

            var note = new Note()
            {
                Description = description,
                Image = image,
                CreatedOn = DateTime.Now,
                NoteCategoryId = categoryId,
                UserId = userId,
                LogbookId = (int)logbookId
            };

            if (categoryId != null)
            {
                var noteCategory = await this.context.NoteCategories.FindAsync(categoryId);

                if (noteCategory.Name == "Task")
                {
                    note.IsActiveTask = true;
                }
            }

            await this.context.Notes.AddAsync(note);
            await this.context.SaveChangesAsync();

            var result = await this.context.Notes
                                           .Include(x => x.User)
                                           .Include(x => x.NoteCategory)
                                           .FirstOrDefaultAsync(x => x.Id == note.Id);
            return result.ToDTO();
        }

        
        public async Task<NoteDTO> DisableNoteActiveStatus(int noteId, string userId)

        {
            var note = await this.context.Notes
                                         .Include(x => x.User)
                                         .Include(x => x.NoteCategory)
                                         .FirstOrDefaultAsync(x => x.Id == noteId);
            if (note == null)
            {
                throw new ArgumentException(ServicesConstants.NoteDoesNotExists);
            }

            var logbookId = note.LogbookId;

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToEditNote);
            }
            note.IsActiveTask = false;
            this.context.Notes.Update(note);
            await this.context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> EditNoteAsync(int noteId, string userId, string description,
                                              string image, int? categoryId)
        {
             var note = await this.context.Notes
                                           .Include(x => x.User)
                                           .Include(x => x.NoteCategory)
                                           .FirstOrDefaultAsync(x => x.Id == noteId);
            if (note == null)
            {
                throw new ArgumentException(ServicesConstants.NoteDoesNotExists);
            }


            if (note.UserId != userId)
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToEditNote);
            }

            if (description != null)
            {
                note.Description = description;
            }

            if (image != null)
            {
                note.Image = image;
            }

            if (categoryId != null)
            {
                note.NoteCategoryId = categoryId;
                var noteCategory = await this.context.NoteCategories.FindAsync(categoryId);

                if (noteCategory.Name == "Task")
                {
                    note.IsActiveTask = true;
                }
            }
            await this.context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesPerDayAsync(string userId, int logbookId)
        {
            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }

            var result = await this.context.Notes
                 .Include(mt => mt.NoteCategory)
                 .Include(mt => mt.User)
                 .Include(mt => mt.Logbook)
                    .ThenInclude(lb => lb.UsersLogbooks)
                 .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn.Day == DateTime.Now.Day)
                 .OrderByDescending(x => x.CreatedOn)
                 .ToListAsync();

              return result.Select(x => x.ToDTO()).ToList();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days)

        {
            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }

            return await this.context.Notes
                           .Include(mt => mt.Logbook)
                           .Include(mt => mt.NoteCategory)
                           .Include(mt => mt.User)
                               .ThenInclude(lb => lb.UsersLogbooks)
                           .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= DateTime.Now.AddDays(-days))
                           .OrderByDescending(x => x.CreatedOn)
                           .Select(x => x.ToDTO())
                           .ToListAsync();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDateRangeAsync(string userId, int logbookId,
                                                                                         DateTime startDate, DateTime endDate)
        {
           
            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }


            return await this.context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId)
                                                                                        
        {

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }

            return await this.context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.IsActiveTask)
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();

        }

        public async Task<IReadOnlyCollection<NoteDTO>> SearchNotesContainsStringAsync(string userId, int logbookId, string criteria)

        {
            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToViewNotes);
            }

            return await this.context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(criteria.ToLower().Replace(" ", string.Empty)))
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();
        }
    }
}

