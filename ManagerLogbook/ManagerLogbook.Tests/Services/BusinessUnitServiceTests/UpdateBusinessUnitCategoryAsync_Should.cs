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
    public class UpdateBusinessUnitCategoryAsync_Should
    {
        [TestMethod]
        public async Task Succeed_UpdateBusinessUnitCategoryName()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_UpdateBusinessUnitCategoryName));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitCategory = await sut.UpdateBusinessUnitCategoryAsync(1, "Hostel");

                mockBusinessValidator.Verify(x => x.IsNameInRange("Hostel"), Times.Exactly(1));

                Assert.AreEqual(businessUnitCategory.Name, "Hostel");
            }
        }
    }
}
