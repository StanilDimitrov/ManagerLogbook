using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Bll.Contracts
{
    public interface IBusinessUnitEngine
    {
        Task<BusinessUnitDTO> CreateBusinnesUnitAsync(BusinessUnitModel model);

        Task<BusinessUnitDTO> UpdateBusinessUnitAsync(BusinessUnitModel model);

        Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksForBusinessUnitAsync(int businessUnitId);

        Task<UserDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId);

        Task<UserDTO> RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId);
    }
}
