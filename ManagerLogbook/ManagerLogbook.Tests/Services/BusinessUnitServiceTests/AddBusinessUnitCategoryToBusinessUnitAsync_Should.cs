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
    public class AddBusinessUnitCategoryToBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task Should_AddBusinessUnitCategoryToBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_AddBusinessUnitCategoryToBusinessUnitAsync));

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

                var businessUnitDTO = await sut.AddBusinessUnitCategoryToBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01().Id, TestHelperBusinessUnit.TestBusinessUnit01().Id);

                Assert.AreEqual(businessUnitDTO.Id, 1);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitNotExists_AddBusinessUnitCategoryToBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitNotExists_AddBusinessUnitCategoryToBusinessUnitAsync));

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
                               
                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddBusinessUnitCategoryToBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01().Id, 2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitCategoryNotExists_AddBusinessUnitCategoryToBusinessUnitAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitCategoryNotExists_AddBusinessUnitCategoryToBusinessUnitAsync));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddBusinessUnitCategoryToBusinessUnitAsync(2, TestHelperBusinessUnit.TestBusinessUnit01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitCategoryNotFound));
            }
        }
    }
}
