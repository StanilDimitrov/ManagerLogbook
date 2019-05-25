using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
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

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            var note = await this.context.Notes
                                           .Where(x => x.Id == id)
                                           .Include(x => x.User)
                                           .FirstOrDefaultAsync();
            return note;
        }

        public async Task<Note> CreateNoteAsync(string description,
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
                NoteCategoryId = categoryId
            };

            if (categoryId != null)
            {
                var noteCategory = await this.context.NoteCategories.FindAsync(categoryId);

                if (noteCategory.Type == "Task")
                {
                    note.IsActiveTask = true;
                }
            }

            await this.context.Notes.AddAsync(note);
            return note;
        }

        public async Task<Note> ChangeNoteStatusAsync(Note note, string userId)

        {
            if (note.UserId != userId)
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToEditTask);
            }

            note.IsActiveTask = false;
            await this.context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> EditNoteAsync(Note note, string userId, string description,
                                              string image, int? categoryId)
        {

            if (note.UserId != userId)
            {
                throw new ArgumentException(ServicesConstants.UserIsNotAuthorizedToEditTask);
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

                if (noteCategory.Type == "Task")
                {
                    note.IsActiveTask = true;
                }
            }

            await this.context.SaveChangesAsync();
            return note;
        }

        public async Task<IReadOnlyCollection<Note>> ShowLogbookNotesPerDayAsync(string userId, int logbookId)
        {
            var tasks = await this.context.Notes
                .Include(mt => mt.Logbook)
                    .ThenInclude(lb => lb.UsersLogbooks)
                .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn.Day == DateTime.Now.Day)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            return tasks;
        }

        public async Task<IReadOnlyCollection<Note>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days)

        {
            var tasks = await this.context.Notes
                           .Include(mt => mt.Logbook)
                               .ThenInclude(lb => lb.UsersLogbooks)
                           .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= DateTime.Now.AddDays(-days))
                           .OrderByDescending(x => x.CreatedOn)
                           .ToListAsync();
            return tasks;
        }

        public async Task<IReadOnlyCollection<Note>> ShowLogbookNotesForDateRangeAsync(string userId, int logbookId,
                                                                                         DateTime startDate, DateTime endDate)
        {
            var tasks = await this.context.Notes
               .Include(mt => mt.Logbook)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
               .OrderByDescending(x => x.CreatedOn)
               .ToListAsync();

            return tasks;
        }

        public async Task<IReadOnlyCollection<Note>> SearchNotesContainsStringAsync(string userId, int logbookId, string criteria)

        {
            var tasks = await this.context.Notes
               .Include(mt => mt.Logbook)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(criteria.ToLower().Replace(" ", string.Empty)))
               .OrderByDescending(x => x.CreatedOn)
               .ToListAsync();

            return tasks;
        }
    }
}

