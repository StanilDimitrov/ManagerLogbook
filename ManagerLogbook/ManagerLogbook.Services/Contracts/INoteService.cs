using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface INoteService
    {
        
        Task<NoteDTO> CreateNoteAsync(string userId, int logbookId, string description, string image, int? categoryId);

        Task<NoteDTO> DisableNoteActiveStatus(int noteId, string userId);

        Task<NoteDTO> EditNoteAsync(int noteId, string userId,
                                 string description, string image, int? categoryId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesPerDayAsync(string userId, int logbookId);
     
        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDateRangeAsync(string userId, int logbookId,
                                                                          DateTime startDate, DateTime endDate);

        Task<IReadOnlyCollection<NoteDTO>> SearchNotesContainsStringAsync(string userId, int logbookId, string criteria);

    }
}
