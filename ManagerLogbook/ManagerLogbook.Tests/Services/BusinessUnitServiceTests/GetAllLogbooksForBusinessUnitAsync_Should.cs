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
    public class GetAllLogbooksForBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task Should_GetAllLogbooksForBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_GetAllLogbooksForBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook02());
                await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook03());
                await arrangeContext.Notes.AddAsync(TestHelperBusinessUnit.TestNote01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var logbooks = await sut.GetAllLogbooksForBusinessUnitAsync(1);

                Assert.AreEqual(logbooks.Count, 2);
            }
        }

        [TestMethod]
        public async Task ThrowsException_GetLogbooksForBusinessUnitNotFound()
        {
            var options = TestUtils.GetOptions(nameof(Should_GetAllLogbooksForBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());

                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook02());
                await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook03());
                await arrangeContext.Notes.AddAsync(TestHelperBusinessUnit.TestNote01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetAllLogbooksForBusinessUnitAsync(2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }
    }
}
