using ManagerLogbook.Services.Bll.Contracts;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll
{
    public class BusinessUnitEngine : IBusinessUnitEngine
    {
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;

        public BusinessUnitEngine(IUserService userService, IBusinessUnitService businessUnitService)
        {
            _userService = userService;
            _businessUnitService = businessUnitService;
        }

        public async Task<BusinessUnitDTO> CreateBusinnesUnitAsync(BusinessUnitModel model)
        {
            await _businessUnitService.CheckIfBrandNameExist(model.Name);

            var businessUnitDto = await _businessUnitService.CreateBusinnesUnitAsync(model);
            return businessUnitDto;
        }

        public async Task<BusinessUnitDTO> UpdateBusinessUnitAsync(BusinessUnitModel model)
        {
            var businessUnit = await _businessUnitService.GetBusinessUnitAsync(model.Id);

            if (businessUnit.Name != model.Name)
            {
                await _businessUnitService.CheckIfBrandNameExist(model.Name);
            }

            var businessUnitDto = await _businessUnitService.UpdateBusinessUnitAsync(model, businessUnit);
            return businessUnitDto;
        }

        public async Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksForBusinessUnitAsync(int businessUnitId)
        {
            await _businessUnitService.GetBusinessUnitAsync(businessUnitId);

            var result = await _businessUnitService.GetLogbooksForBusinessUnitAsync(businessUnitId);
            return result;
        }

        public async Task<UserDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            await _businessUnitService.GetBusinessUnitAsync(businessUnitId);
            var moderator = await _userService.GetUserAsync(moderatorId);

            return await _businessUnitService.AddModeratorToBusinessUnitsAsync(moderator, businessUnitId);
        }

        public async Task<UserDTO> RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId)
        {
            await _businessUnitService.GetBusinessUnitAsync(businessUnitId);
            var moderator = await _userService.GetUserAsync(moderatorId);

            return await _businessUnitService.RemoveModeratorFromBusinessUnitsAsync(moderator, businessUnitId);
        }
    }
}
