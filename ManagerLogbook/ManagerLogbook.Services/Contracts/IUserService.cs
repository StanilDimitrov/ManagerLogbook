using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(string userId);

        Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId);

        Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsNotPresentInBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsPresentInBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<UserDTO>> GetAllManagersNotPresentInLogbookAsync(int logbookId);

        Task<IReadOnlyCollection<UserDTO>> GetAllManagersPresentInLogbookAsync(int logbookId);
    }
}
