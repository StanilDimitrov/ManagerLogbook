using ManagerLogbook.Services.DTOs;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserById(string userId);
    }
}
