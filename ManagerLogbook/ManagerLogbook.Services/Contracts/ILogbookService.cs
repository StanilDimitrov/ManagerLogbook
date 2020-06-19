using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface ILogbookService
    {
        Task<Logbook> GetLogbookAsync(int logbookId);

        Task<LogbookDTO> CreateLogbookAsync(LogbookModel model);

        Task<LogbookDTO> GetLogbookDetailsAsync(int logbookId);

        Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model, Logbook logbook);

        Task<UserDTO> AddManagerToLogbookAsync(User manager, int logbookId);

        Task<UserDTO> RemoveManagerFromLogbookAsync(string managerId, int logbookId);

        Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksByUserAsync(string userId);

        Task<LogbookDTO> AddLogbookToBusinessUnitAsync(Logbook logbook, int businessUnitId);

        Task<bool> CheckIfLogbookNameExist(string logbookName);
    }
}
