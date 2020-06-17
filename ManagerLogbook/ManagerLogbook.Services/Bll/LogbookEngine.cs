using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
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

            return await _logbookService.UpdateLogbookAsync(model, logbook);
        }
    }
}
