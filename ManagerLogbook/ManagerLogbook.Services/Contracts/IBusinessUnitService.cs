using ManagerLogbook.Data.Models;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IBusinessUnitService
    {
        Task<BusinessUnit> CreateBusinnesUnitAsync(string brandName, string address, string phoneNumber, string email);

        Task<BusinessUnit> IsBusinessUnitExists(string brandName);

        Task<BusinessUnit> UpdateBusinessUnitAsync(BusinessUnit businessUnit, string brandName, string address, string phoneNumber, string email, string picture);

        Task<BusinessUnit> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId);
    }
}
