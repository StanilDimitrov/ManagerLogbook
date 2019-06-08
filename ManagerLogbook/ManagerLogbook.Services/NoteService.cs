using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
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
            var note = await this.context.Notes
                                         .Include(x => x.User)
                                         .Include(x => x.NoteCategory)
                                         .FirstOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NotNotFound);
            }

            return note.ToDTO();
        }

        public async Task<NoteDTO> CreateNoteAsync(string userId, int logbookId, string description, 
                                             string image, int? categoryId)
        {
            validator.IsDescriptionIsNullOrEmpty(description);
            validator.IsDescriptionInRange(description);

            var user = await this.context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            var logbook = await this.context.Logbooks.FindAsync(logbookId);
            if (logbook == null)
            {
                throw new NotFoundException(ServicesConstants.LogbookNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserNotManagerOfLogbook, user.UserName, logbook.Name));
            }

            var note = new Note()
            {
                Description = description,
                Image = image,
                CreatedOn = DateTime.Now,
                NoteCategoryId = categoryId,
                UserId = userId,
                LogbookId = logbookId
            };

            if (categoryId != null)
            {
                var noteCategory = await this.context.NoteCategories.FindAsync(categoryId);
                if (noteCategory == null)
                {
                    throw new NotFoundException(ServicesConstants.NoteCategoryDoesNotExists);
                }

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

        
        public async Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId)

        {
            var note = await this.context.Notes
                                         .Include(x => x.User)
                                         .Include(x => x.NoteCategory)
                                         .FirstOrDefaultAsync(x => x.Id == noteId);
            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NotNotFound);
            }
            var user = await this.context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }
            var logbookId = note.LogbookId;
            

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote, user.UserName));
            }
            note.IsActiveTask = false;
            this.context.Notes.Update(note);
            await this.context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<NoteDTO> EditNoteAsync(int noteId, string userId, string description,
                                              string image, int? categoryId)
        {
            validator.IsDescriptionIsNullOrEmpty(description);
            validator.IsDescriptionInRange(description);

            var note = await this.context.Notes
                                           .Include(x => x.User)
                                           .Include(x => x.NoteCategory)
                                           .FirstOrDefaultAsync(x => x.Id == noteId);
            if (note == null)
            {
                throw new NotFoundException(ServicesConstants.NotNotFound);
            }

            var user = await this.context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            if (note.UserId != userId)
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote,user.UserName));
            }

            if (description != null)
            {
                validator.IsDescriptionInRange(description);
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

                if (noteCategory == null)
                {
                    throw new NotFoundException(ServicesConstants.NoteCategoryDoesNotExists);
                }
                if (noteCategory.Name == "Task")
                {
                    note.IsActiveTask = true;
                }
            }
           
            await this.context.SaveChangesAsync();

            return note.ToDTO();
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days)

        {
            var user = await this.context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes, user.UserName));
            }

            var result = await this.context.Notes
                           .Include(mt => mt.Logbook)
                           .Include(mt => mt.NoteCategory)
                           .Include(mt => mt.User)
                               .ThenInclude(lb => lb.UsersLogbooks)
                           .Where(mt => mt.LogbookId == logbookId && mt.CreatedOn.Date >= DateTime.Now.Date.AddDays(-days))
                           .OrderByDescending(x => x.CreatedOn)
                           .Select(x => x.ToDTO())
                           .ToListAsync();

            return result;
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId)
                                                                                        
        {
            var user = await this.context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes, user.UserName));
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

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId)

        {
            var user = await this.context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes, user.UserName));
            }

            return await this.context.Notes
               .Include(mt => mt.Logbook)
               .Include(mt => mt.NoteCategory)
               .Include(mt => mt.User)
                   .ThenInclude(lb => lb.UsersLogbooks)
               .Where(mt => mt.LogbookId == logbookId)
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => x.ToDTO())
               .ToListAsync();

        }

        public async Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, 
                                                                                             DateTime startDate, DateTime endDate, int? categoryId, string criteria)

        {
            var user = await this.context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            if (!this.context.UsersLogbooks.Any(x => x.UserId == userId && x.LogbookId == logbookId))
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes, user.UserName));
            }

            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }


            //IQueryable<int> searchCollectionInt = this.context.BusinessUnits.Where(n => n.Name.ToLower().Contains(searchCriteria.ToLower())).Select(x => x.Id);

            //IQueryable<int> searchCategoryCollectionInt = this.context.BusinessUnits.Where(buc => buc.BusinessUnitCategoryId == businessUnitCategoryId).Select(x => x.Id);

            //IQueryable<int> searchTownCollectionInt = this.context.BusinessUnits.Where(t => t.TownId == townId).Select(x => x.Id);

            //var searchInt = searchTownCollectionInt.Intersect(searchCollectionInt.Intersect(searchCategoryCollectionInt));

            //var businessUnitsIDs = new List<BusinessUnit>();
            //foreach (var id in searchInt)
            //{
            //    var currentBusinessUnit = await this.context.BusinessUnits.FindAsync(id);
            //    businessUnitsIDs.Add(currentBusinessUnit);
            //}

            //var businessUnitsDTOid = businessUnitsIDs.Select(x => x.ToDTO()).ToList();

            //return businessUnitsDTOid;


            IQueryable<Note> searchCollection = this.context.Notes
               
               .Where(mt => mt.LogbookId == logbookId && mt.Description.ToLower().Replace(" ", string.Empty).Contains(criteria.ToLower().Replace(" ", string.Empty)) && mt.CreatedOn >= startDate && mt.CreatedOn <= endDate)
               .OrderByDescending(x => x.CreatedOn);

            IQueryable<Note> searchCategoryCollection = this.context.Notes
               .Where(mt => mt.NoteCategoryId == categoryId)
               .OrderByDescending(x => x.CreatedOn);

            var search = searchCollection.Intersect(searchCategoryCollection);

            var notesDTO = await search
                              .Include(mt => mt.Logbook)
                              .Include(mt => mt.NoteCategory)
                              .Include(mt => mt.User)
                                .ThenInclude(lb => lb.UsersLogbooks)
                              .Select(x => x.ToDTO())
                              .ToListAsync();

            return notesDTO;
        }


        public async Task<IReadOnlyCollection<NoteGategoryDTO>> GetNoteCategoriesAsync()

        {

           var result = await this.context.NoteCategories
                                          .Select(x => x.ToDTO())
                                          .ToListAsync();
           return result;
        }

        public async Task<IReadOnlyCollection<NoteDTO>> Get15NotesByIdAsync(int currPage, int logbookId)
        {
            if (currPage == 1)
            {
               return await context.Notes
                                         .Where(x => x.LogbookId == logbookId)
                                         .OrderByDescending(x => x.Id)
                                         .Take(15)
                                         .Select(x => x.ToDTO())
                                         .ToListAsync();

            }
            else
            {
                return  await context
                                    .Notes
                                    .Where(x => x.LogbookId == logbookId)
                                    .OrderByDescending(x => x.Id)
                                    .Skip((currPage - 1) * 15)
                                    .Take(15)
                                    .Select(x => x.ToDTO())
                                    .ToListAsync();

            }
        }

        public async Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId)
        {
            var allNotesCount = await context.Notes.Where(x => x.LogbookId == logbookId).CountAsync();

            int pageCount = (allNotesCount - 1) / notesPerPage + 1;

            return pageCount;
        }
    }
}

