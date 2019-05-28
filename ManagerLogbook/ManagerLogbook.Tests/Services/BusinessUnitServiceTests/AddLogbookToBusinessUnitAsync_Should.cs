using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
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
    public class AddLogbookToBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task Should_AddLogbookToBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_AddLogbookToBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnit = await sut.AddLogbookToBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit01().Id, TestHelperBusinessUnit.TestLogbook01().Id);


                Assert.AreEqual(businessUnit.Logbooks.Count, 1);
            }
        }
    }
}
