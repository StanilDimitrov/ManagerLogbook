using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IBusinessUnitService
    {
        Task<BusinessUnitDTO> CreateBusinnesUnitAsync(BusinessUnitModel model);

        Task<BusinessUnitDTO> UpdateBusinessUnitAsync(BusinessUnitModel model);

        Task<BusinessUnitDTO> GetBusinessUnitDtoAsync(int businessUnitId);

        Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksForBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetBusinessUnitsAsync();

        Task<IReadOnlyCollection<TownDTO>> GetAllTownsAsync();

        Task<UserDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId);

        Task<UserDTO> RemoveModeratorFromBusinessUnitsAsync(string moderatorId, int businessUnitId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> SearchBusinessUnitsAsync(string searchCriteria, int? businessUnitCategoryId, int? townId);

        Task<IReadOnlyCollection<BusinessUnitCategoryDTO>> GetBusinessUnitsCategoriesAsync();

        Task<IReadOnlyDictionary<string, int>> GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();

        Task<BusinessUnitDTO> AddLikeToBusinessUnitAsync(int businessUnitId);
    }
}
