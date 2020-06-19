using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface INoteService
    {
        Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId);

        Task<NoteDTO> DeactivateNoteActiveStatus(Note note, bool isActiveStatus);

        Task<NoteDTO> UpdateNoteAsync(NoteModel model, Note note);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(int logbookId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(int logbookId, int days);

        Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, SearchNoteModel model);
                                                                            
        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(int logbookId);

        Task<IReadOnlyCollection<NoteGategoryDTO>> GetNoteCategoriesAsync();

        Task<IReadOnlyCollection<NoteDTO>> Get15NotesByIdAsync(int currPage, int logbookId);

        Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId);

        Task<int> GetPageCountForNotesAsync(int notesPerPage, int logbookId, string searchPhrase);

        Task<bool> CheckIfUserIsAuthorized(string userId, int logbookId);

        Task<NoteCategory> GetNoteCategoryAsync(int id);

        Task<Note> GetNoteAsync(int id);
    }
}
