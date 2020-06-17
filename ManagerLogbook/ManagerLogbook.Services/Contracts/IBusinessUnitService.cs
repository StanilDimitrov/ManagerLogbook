using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IBusinessUnitService
    {
        Task<BusinessUnitDTO> CreateBusinnesUnitAsync(BusinessUnitModel model);

        Task<BusinessUnitDTO> UpdateBusinessUnitAsync(BusinessUnitModel model, BusinessUnit businessUnit);

        Task<BusinessUnitDTO> GetBusinessUnitDtoAsync(int businessUnitId);

        Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksForBusinessUnitAsync(int businessUnitId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetBusinessUnitsAsync();

        Task<IReadOnlyCollection<TownDTO>> GetTownsAsync();

        Task<UserDTO> AddModeratorToBusinessUnitsAsync(User moderator, int businessUnitId);

        Task<UserDTO> RemoveModeratorFromBusinessUnitsAsync(User moderator, int businessUnitId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> SearchBusinessUnitsAsync(string searchCriteria, int? businessUnitCategoryId, int? townId);

        Task<IReadOnlyCollection<BusinessUnitCategoryDTO>> GetBusinessUnitsCategoriesAsync();

        Task<IReadOnlyDictionary<string, int>> GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();

        Task<BusinessUnitDTO> AddLikeToBusinessUnitAsync(int businessUnitId);

        Task<BusinessUnit> GetBusinessUnitAsync(int businessUnitId);

        Task<bool> CheckIfBrandNameExist(string brandName);
    }
}
