using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll
{
    public class LogbookEngine : ILogbookEngine
    {
        private readonly ILogbookService _logbookService;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;

        public LogbookEngine(ILogbookService logbookService, IBusinessUnitService businessUnitService, IUserService userService)
        {
            _logbookService = logbookService;
            _businessUnitService = businessUnitService;
            _userService = userService;
        }

        public async Task<LogbookDTO> CreateLogbookAsync(LogbookModel model)
        {
            await _logbookService.CheckIfLogbookNameExist(model.Name);
            await _businessUnitService.GetBusinessUnitAsync(model.BusinessUnitId.Value);

            return await _logbookService.CreateLogbookAsync(model);
        }

        public async Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model)
        {
            var logbook = await _logbookService.GetLogbookAsync(model.Id);

            if (model.Name != logbook.Name)
            {
                await _logbookService.CheckIfLogbookNameExist(model.Name);
            }

            if (model.BusinessUnitId.HasValue)
            {
                await _businessUnitService.GetBusinessUnitAsync(model.BusinessUnitId.Value);
            }

            return await _logbookService.UpdateLogbookAsync(model, logbook);
        }

        public async Task<UserDTO> AddManagerToLogbookAsync(string managerId, int logbookId)
        {
            var logbook = await _logbookService.GetLogbookAsync(logbookId);
            var manager = await _userService.GetUserAsync(managerId);

            bool isUserManagerOfLogbook = await _userService.CheckIfUserIsManagerOfLogbook(managerId, logbookId);
            if (isUserManagerOfLogbook)
            {
                throw new AlreadyExistsException(string.Format(ServicesConstants.ManagerIsAlreadyInLogbook, manager.UserName, logbook.Name));
            }

            return await _logbookService.AddManagerToLogbookAsync(manager, logbookId);
        }


        public async Task<LogbookDTO> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId)
        {
            var logbook = await _logbookService.GetLogbookAsync(logbookId);
            await _businessUnitService.GetBusinessUnitAsync(businessUnitId);

            return await _logbookService.AddLogbookToBusinessUnitAsync(logbook, businessUnitId);
        }
    }
}
