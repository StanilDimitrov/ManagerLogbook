using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserDtoAsync(string userId);

        Task<User> GetUserAsync(string userId);

        Task<UserDTO> SwitchLogbookAsync(User user, int logbookId);

        Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsNotPresentInBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<UserDTO>> GetAllModeratorsPresentInBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<UserDTO>> GetAllManagersNotPresentInLogbookAsync(int logbookId);

        Task<IReadOnlyCollection<UserDTO>> GetAllManagersPresentInLogbookAsync(int logbookId);

        Task<bool> CheckIfUserIsManagerOfLogbook(string userId, int logbookId);
    }
}
