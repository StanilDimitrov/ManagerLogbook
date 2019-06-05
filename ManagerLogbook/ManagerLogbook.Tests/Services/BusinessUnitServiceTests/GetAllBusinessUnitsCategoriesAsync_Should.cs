using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.BusinessUnitServiceTests
{
    [TestClass]
    public class GetAllBusinessUnitsCategoriesAsync_Should
    {
        [TestMethod]
        public async Task Should_GetAllBusinessUnitsCategoriesAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_GetAllBusinessUnitsCategoriesAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {               
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory02());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory03());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitsCategories = await sut.GetAllBusinessUnitsCategoriesAsync();

                Assert.AreEqual(businessUnitsCategories.Count, 3);
            }
        }
    }
}
