using ManagerLogbook.Services.DTOs;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll.Contracts
{
    public interface IUserEngine
    {
        Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId);
    }
}
