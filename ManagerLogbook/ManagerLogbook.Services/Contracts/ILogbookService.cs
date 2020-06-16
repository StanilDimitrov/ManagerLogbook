using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface ILogbookService
    {
        Task<LogbookDTO> CreateLogbookAsync(LogbookModel model);

        Task<LogbookDTO> GetLogbookById(int logbookId);

        Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model);

        Task<UserDTO> AddManagerToLogbookAsync(string managerId, int logbookId);

        Task<UserDTO> RemoveManagerFromLogbookAsync(string managerId, int logbookId);

        Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksByUserAsync(string userId);

        Task<LogbookDTO> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId);
    }
}
