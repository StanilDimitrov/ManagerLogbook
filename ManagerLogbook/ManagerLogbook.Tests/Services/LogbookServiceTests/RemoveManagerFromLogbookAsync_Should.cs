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
    public class RemoveManagerFromLogbookAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeptionWhenManagerIsNotPresentInLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenManagerIsNotPresentInLogbook));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks02());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.RemoveManagerFromLogbookAsync(TestHelpersLogbook.TestUser01().Id, TestHelpersLogbook.TestLogbook01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.ManagerIsNotPresentInLogbook, TestHelpersLogbook.TestUser01().UserName, TestHelpersLogbook.TestLogbook01().Name));
            }
        }

        [TestMethod]
        public async Task Succeed_ReturnLogbookWhenManagerWasRemoved()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnLogbookWhenManagerWasRemoved));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser02());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook04());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks04());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                await sut.RemoveManagerFromLogbookAsync(TestHelpersLogbook.TestUser01().Id, TestHelpersLogbook.TestLogbook04().Id);
                                
                Assert.AreEqual(assertContext.UsersLogbooks.Select(x => x.LogbookId).Count(), 0);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenManagerWasNotFoundByRemoveManagerFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenManagerWasNotFoundByRemoveManagerFromLogbook));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.RemoveManagerFromLogbookAsync("2", TestHelpersLogbook.TestLogbook01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookWasNotFoundByRemoveManagerFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookWasNotFoundByRemoveManagerFromLogbook));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.RemoveManagerFromLogbookAsync(TestHelpersLogbook.TestUser01().Id, 2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }
    }
}
