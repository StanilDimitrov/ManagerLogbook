using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IBusinessUnitService
    {
        Task<BusinessUnitDTO> CreateBusinnesUnitAsync(string brandName, string address, string phoneNumber, string email, string infomration, int businessUnitCategoryId, int townId);

        Task<BusinessUnitDTO> GetBusinessUnitById(int businessUnitId);

        Task<BusinessUnitDTO> UpdateBusinessUnitAsync(int businessUnitId, string brandName, string address, string phoneNumber, string infomration, string email, string picture);

        Task<IReadOnlyCollection<LogbookDTO>> GetAllLogbooksForBusinessUnitAsync(int businessUnitId);

        Task<BusinessUnitCategoryDTO> CreateBusinessUnitCategoryAsync(string businessUnitCategoryName);

        Task<BusinessUnitCategoryDTO> UpdateBusinessUnitCategoryAsync(int businessUnitCategoryId, string newBusinessUnitCategoryName);

        Task<BusinessUnitDTO> AddBusinessUnitCategoryToBusinessUnitAsync(int businessUnitCategoryId, int businessUnitId);

        Task<BusinessUnitCategoryDTO> GetBusinessUnitCategoryByIdAsync(int businessUnitCategoryId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetAllBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> GetAllBusinessUnitsAsync();

        Task<IReadOnlyCollection<TownDTO>> GetAllTownsAsync();

        Task<BusinessUnitDTO> AddModeratorToBusinessUnitsAsync(string moderatorId, int businessUnitId);

        Task<IReadOnlyCollection<BusinessUnitDTO>> SearchBusinessUnitsAsync(string searchCriteria, int businessUnitCategoryId, int townId);

        Task<IReadOnlyCollection<BusinessUnitCategoryDTO>> GetAllBusinessUnitsCategoriesAsync();
    }
}
