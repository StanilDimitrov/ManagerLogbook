﻿using ManagerLogbook.Data;
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
    public class AddModeratorToBusinessUnitsAsync_Should
    {
        [TestMethod]
        public async Task Should_AddModeratorToBusinessUnitsAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_AddModeratorToBusinessUnitsAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Users.AddAsync(TestHelperBusinessUnit.TestUser01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitDTO = await sut.AddModeratorToBusinessUnitsAsync(TestHelperBusinessUnit.TestUser01().Id, TestHelperBusinessUnit.TestBusinessUnit01().Id);

                var moderatorUser = await assertContext.Users.FindAsync(TestHelperBusinessUnit.TestUser01().Id);

                Assert.AreEqual(moderatorUser.BusinessUnitId, TestHelperBusinessUnit.TestBusinessUnit01().Id);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitDoesExists_AddModeratorToBusinessUnitsAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitDoesExists_AddModeratorToBusinessUnitsAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Users.AddAsync(TestHelperBusinessUnit.TestUser01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnit = await sut.AddModeratorToBusinessUnitsAsync(TestHelperBusinessUnit.TestUser01().Id, TestHelperBusinessUnit.TestBusinessUnit01().Id);

                var moderatorUser = await assertContext.Users.FindAsync(TestHelperBusinessUnit.TestUser01().Id);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddModeratorToBusinessUnitsAsync(TestHelperBusinessUnit.TestUser01().Id, 2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenModeratorUserDoesExists_AddModeratorToBusinessUnitsAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenModeratorUserDoesExists_AddModeratorToBusinessUnitsAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Users.AddAsync(TestHelperBusinessUnit.TestUser01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnit = await sut.AddModeratorToBusinessUnitsAsync(TestHelperBusinessUnit.TestUser01().Id, TestHelperBusinessUnit.TestBusinessUnit01().Id);

                var moderatorUser = await assertContext.Users.FindAsync(TestHelperBusinessUnit.TestUser01().Id);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddModeratorToBusinessUnitsAsync("11", TestHelperBusinessUnit.TestBusinessUnit01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotFound));
            }
        }
    }
}
