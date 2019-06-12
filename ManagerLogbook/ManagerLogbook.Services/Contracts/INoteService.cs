using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface INoteService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NoteDTO> GetNoteByIdAsync(int id);
            
        Task<NoteDTO> CreateNoteAsync(string userId, int logbookId, string description, string image, int? categoryId);

        Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId);

        Task<NoteDTO> EditNoteAsync(int noteId, string userId,
                                 string description, string image, int? categoryId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days);

        Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, DateTime startDate,
                                                                                 DateTime endDate, int? categoryId, string criteria, int? daysBefore, int currPage = 1);
        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteGategoryDTO>> GetNoteCategoriesAsync();

        Task<IReadOnlyCollection<NoteDTO>> Get15NotesByIdAsync(int currPage, int logbookId);

        Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId);
    }
}
