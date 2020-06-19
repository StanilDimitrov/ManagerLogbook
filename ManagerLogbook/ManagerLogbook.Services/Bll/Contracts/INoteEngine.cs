using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll.Contracts
{
    public interface INoteEngine
    {
        Task<NoteDTO> CreateNoteAsync(NoteModel model, int logbookId, string userId);

        Task<NoteDTO> UpdateNoteAsync(NoteModel model);

        Task<NoteDTO> DeactivateNoteActiveStatus(int noteId, string userId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesForDaysBeforeAsync(string userId, int logbookId, int days);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesWithActiveStatusAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteDTO>> ShowLogbookNotesAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<NoteDTO>> SearchNotesAsync(string userId, int logbookId, SearchNoteModel searchModel);
    }
}
