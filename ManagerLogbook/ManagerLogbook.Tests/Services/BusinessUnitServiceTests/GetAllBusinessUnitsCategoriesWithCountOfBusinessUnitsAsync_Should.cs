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
    public class GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync_Should
    {
        [TestMethod]
        public async Task Should_GetAllBusinessUnitsCategoriesWithCountOfBusinessUnits()
        {
            var options = TestUtils.GetOptions(nameof(Should_GetAllBusinessUnitsCategoriesWithCountOfBusinessUnits));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory02());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory03());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit02());
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit03());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var categoriesWithCountOfBusinessUnits = await sut.GetAllBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();

                Assert.AreEqual(categoriesWithCountOfBusinessUnits[TestHelperBusinessUnit.TestBusinessUnitCategory01().Name], 2);
                Assert.AreEqual(categoriesWithCountOfBusinessUnits[TestHelperBusinessUnit.TestBusinessUnitCategory02().Name], 1);

            }
        }
    }
}
