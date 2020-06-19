using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll
{
    public class NoteEngine : INoteEngine
    {
        private readonly INoteService _noteService;
        private readonly IUserService _userService;

        public NoteEngine(INoteService noteService, IUserService userService)
        {
            _noteService = noteService;
            _userService = userService;
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId)
        {
            await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            if (model.CategoryId.HasValue)
            {
                var noteCategory = await _noteService.GetNoteCategoryAsync(model.CategoryId.Value);

                if (noteCategory.Name == "Task")
                {
                    model.IsActiveTask = true;
                }
            }

            return await _noteService.CreateNoteAsync(model, logbookId, userId);
        }

        public async Task<NoteDTO> UpdateNoteAsync(NoteModel model)
        {
            var note = await _noteService.GetNoteAsync(model.Id);
            var user = await _userService.GetUserAsync(model.UserId);

            if (note.UserId != model.UserId)
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote, user.UserName));
            }

            if (model.CategoryId.HasValue)
            {
                var noteCategory = await _noteService.GetNoteCategoryAsync(model.CategoryId.Value);

                if (noteCategory.Name == "Task")
                {
                    model.IsActiveTask = true;
                }
            }

            return await _noteService.UpdateNoteAsync(model, note);
        }

        public async Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId)
        {
            var note = await _noteService.GetNoteAsync(noteId);
            await _noteService.CheckIfUserIsAuthorized(userId, note.LogbookId);

            bool isActiveStatus = false;
            return await _noteService.DeactivateNoteActiveStatus(note, isActiveStatus);
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days)
        {
            await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            return await _noteService.ShowLogbookNotesForDaysBeforeAsync(logbookId, days);
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId)
        {
            await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            return await _noteService.ShowLogbookNotesWithActiveStatusAsync(logbookId);
        }

        public async Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId)
        {
            await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            return await _noteService.ShowLogbookNotesAsync(logbookId);
        }

        public async Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, SearchNoteModel searchModel)
        {
            await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            if (searchModel.EndDate == DateTime.MinValue)
            {
                searchModel.EndDate = DateTime.Now;
            }

            return await _noteService.SearchNotesAsync(userId, logbookId, searchModel);
        }
    }
}
