using ManagerLogbook.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IBusinessUnitService
    {
        Task<BusinessUnit> CreateBusinnesUnitAsync(string brandName, string address, string phoneNumber, string email);

        Task<BusinessUnit> GetBusinessUnitById(int businessUnitId);

        Task<BusinessUnit> UpdateBusinessUnitAsync(BusinessUnit businessUnit, string brandName, string address, string phoneNumber, string email, string picture);

        Task<BusinessUnit> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId);

        Task<IReadOnlyCollection<Logbook>> GetAllLogbooksForBusinessUnitAsync(int businessUnitId);

        Task<BusinessUnitCategory> CreateBusinessUnitCategoryAsync(string businessUnitCategoryName);

        Task<BusinessUnitCategory> UpdateBusinessUnitCategoryAsync(int businessUnitCategoryId, string newBusinessUnitCategoryName);

        Task<BusinessUnit> AddBusinessUnitCategoryToBusinessUnitAsync(int businessUnitCategoryId, int businessUnitId);

        Task<BusinessUnitCategory> GetBusinessUnitCategoryByIdAsync(int businessUnitCategoryId);

        Task<IReadOnlyCollection<BusinessUnit>> GetAllBusinessUnitsByCategoryIdAsync(int businessUnitCategoryId);
    }
}
