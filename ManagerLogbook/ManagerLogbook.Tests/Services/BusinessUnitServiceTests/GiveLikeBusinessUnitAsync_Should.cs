using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.BusinessUnitServiceTests
{
    [TestClass]
    public class GiveLikeBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task GiveLikeBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(GiveLikeBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitDTO = await sut.GiveLikeBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit01().Id);

                Assert.AreEqual(businessUnitDTO.Likes, 1);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitNotExists_GiveLikeBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitNotExists_GiveLikeBusinessUnitAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GiveLikeBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit02().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }
    }
}
