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
    public class AddManagerToLogbookAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeptionWhenManagerIsAlreadyAddedToLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenManagerIsAlreadyAddedToLogbook));

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

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.AddManagerToLogbookAsync(TestHelpersLogbook.TestUser01().Id, 1));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.ManagerIsAlreadyInLogbook, TestHelpersLogbook.TestUser01().UserName, TestHelpersLogbook.TestLogbook01().Name));
            }
        }

        [TestMethod]
        public async Task Succeed_ReturnLogbookWhenManagerIsAdded()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnLogbookWhenManagerIsAdded));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser02());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                await sut.AddManagerToLogbookAsync(TestHelpersLogbook.TestUser02().Id, TestHelpersLogbook.TestLogbook01().Id);

                await assertContext.SaveChangesAsync();

                Assert.AreEqual(assertContext.UsersLogbooks.Select(x=>x.LogbookId).Count(),2);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenManagerWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenManagerIsAlreadyAddedToLogbook));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddManagerToLogbookAsync("2", TestHelpersLogbook.TestLogbook01().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookWasNotFound));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.AddManagerToLogbookAsync(TestHelpersLogbook.TestUser01().Id, 2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }
    }
}
