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

namespace ManagerLogbook.Tests.Services.UserServiceTest
{
    [TestClass]
    public class SwitchLogbookAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsManagerOfLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsManagerOfLogbook));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedRapper = new Mock<IUserServiceWrapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotManagerOfLogbook, TestHelpersNote.TestUser1().UserName, TestHelpersNote.TestLogbook1().Name));

            }
        }

        [TestMethod]
        public async Task ThrowsExeption_WhenLogbookNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsManagerOfLogbook));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceWrapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.LogbookNotFound);
            }
        }

        [TestMethod]
        public async Task ThrowsExeption_WhenUserNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserNotFound));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceWrapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFound);

            }
        }

        [TestMethod]
        public async Task SuccessfullySwitch()
        {
            var options = TestUtils.GetOptions(nameof(SuccessfullySwitch));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceWrapper>();

                var sut = new UserService(assertContext, mockedRapper.Object);

                var userDTO = await sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id,
                                                           TestHelpersNote.TestLogbook1().Id);

                Assert.AreEqual(userDTO.CurrentLogbookId, TestHelpersNote.TestLogbook1().Id);
            }
        }
    }
}
