using ManagerLogbook.Services.DTOs;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(string userId);

        Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId);
    }
}
