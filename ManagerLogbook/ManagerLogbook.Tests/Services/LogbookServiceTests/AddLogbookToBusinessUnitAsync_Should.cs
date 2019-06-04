using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
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
    }
}
