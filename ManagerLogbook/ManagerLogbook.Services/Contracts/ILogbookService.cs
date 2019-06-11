using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface ILogbookService
    {
        Task<LogbookDTO> CreateLogbookAsync(string name, int businessUnitId, string picture);

        Task<LogbookDTO> GetLogbookById(int logbookId);

        Task<LogbookDTO> UpdateLogbookAsync(int logbookId, string name, string picture);

        Task<LogbookDTO> AddManagerToLogbookAsync(string managerId, int logbookId);

        Task<LogbookDTO> RemoveManagerFromLogbookAsync(string managerId, int logbookId);

        Task<IReadOnlyCollection<LogbookDTO>> GetAllLogbooksByUserAsync(string userId);

        Task<LogbookDTO> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId);
    }
}
