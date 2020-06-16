using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface INoteService
    {
        Task<NoteDTO> GetNoteDtoAsync(int id);
            
        Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId);

        Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId);

        Task<NoteDTO> EditNoteAsync(NoteModel model, string userId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days);

        Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, DateTime startDate,
                                                                                 DateTime endDate, int? categoryId, string criteria, int? daysBefore, int currPage = 1);
        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteGategoryDTO>> GetNoteCategoriesAsync();

        Task<IReadOnlyCollection<NoteDTO>> Get15NotesByIdAsync(int currPage, int logbookId);

        Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId);

        Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId, string searchPhrase);
    }
}
