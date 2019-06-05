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
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.LogbookServiceTests
{
    [TestClass]
    public class AddLogbookToBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnLogbookWhenWasAddedToBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnLogbookWhenWasAddedToBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelpersLogbook.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelpersLogbook.TestTown01());

                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                await sut.AddLogbookToBusinessUnitAsync(TestHelpersLogbook.TestLogbook01().Id, TestHelpersLogbook.TestBusinessUnit01().Id);

                await assertContext.SaveChangesAsync();

                Assert.AreEqual(TestHelpersLogbook.TestLogbook01().BusinessUnitId, TestHelpersLogbook.TestBusinessUnit01().Id);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookWasNotFound));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelpersLogbook.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelpersLogbook.TestTown01());

                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddLogbookToBusinessUnitAsync(2, TestHelpersLogbook.TestBusinessUnit01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenBusinessUnitWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenBusinessUnitWasNotFound));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelpersLogbook.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelpersLogbook.TestTown01());

                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddLogbookToBusinessUnitAsync(TestHelpersLogbook.TestLogbook01().Id, 2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }
    }
}
