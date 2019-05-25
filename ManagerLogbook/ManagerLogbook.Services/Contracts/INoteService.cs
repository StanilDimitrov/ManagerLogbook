using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface INoteService
    {
        Task<Note> GetNoteByIdAsync(int id);
     
        Task<Note> CreateNoteAsync(string description, string image, int? categoryId);

        Task<Note> DisableNoteActiveStatus(Note note, string userId);

        Task<Note> EditNoteAsync(Note note, string userId,
                                 string description, string image, int? categoryId);
    
        Task<IReadOnlyCollection<Note>> ShowLogbookNotesPerDayAsync(string userId, int logbookId);
     
        Task<IReadOnlyCollection<Note>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days);

        Task<IReadOnlyCollection<Note>> ShowLogbookNotesForDateRangeAsync(string userId, int logbookId,
                                                                          DateTime startDate, DateTime endDate);

        Task<IReadOnlyCollection<Note>> SearchNotesContainsStringAsync(string userId, int logbookId, string criteria);

      
    }
}
