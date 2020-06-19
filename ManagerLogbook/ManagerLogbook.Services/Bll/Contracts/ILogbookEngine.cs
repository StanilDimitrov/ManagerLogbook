using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll.Contracts
{
    public interface ILogbookEngine
    {
        Task<LogbookDTO> CreateLogbookAsync(LogbookModel model);

        Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model);

        Task<UserDTO> AddManagerToLogbookAsync(string managerId, int logbookId);
    }
}
