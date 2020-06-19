using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Utils;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll
{
    public class UserEngine : IUserEngine
    {
        private readonly IUserService _userService;
        private readonly ILogbookService _logbookService;
        private readonly IUserServiceWrapper _userServiceWrapper;

        public UserEngine(IUserService userService, IUserServiceWrapper userServiceWrapper, ILogbookService logbookService)
        {
            _userService = userService;
            _logbookService = logbookService;
            _userServiceWrapper = userServiceWrapper;
        }

        public async Task<UserDTO> SwitchLogbookAsync(string userId, int logbookId)
        {
            var user = await _userService.GetUserAsync(userId);
            var logbook = await _logbookService.GetLogbookAsync(logbookId);

            var isUserManagerOfLogbook = await _userService.CheckIfUserIsManagerOfLogbook(userId, logbookId);

            if (!isUserManagerOfLogbook)
            {
                throw new NotAuthorizedException(string.Format(ServicesConstants.UserNotManagerOfLogbook, user.UserName, logbook.Name));
            }

            return await _userService.SwitchLogbookAsync(user, logbookId);
        }
    }
}
