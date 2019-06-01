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

namespace ManagerLogbook.Tests.Services.UserServiceTest
{
    [TestClass]
    public class SwitchLogbookAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotFromLogbook));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new UserService(assertContext);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFromLogbook);
                                                          
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
                var mockedValidator = new Mock<IBusinessValidator>();

                var sut = new UserService(assertContext);

                var userDTO = await sut.SwitchLogbookAsync(TestHelpersNote.TestUser1().Id, 
                                                           TestHelpersNote.TestLogbook1().Id);

                Assert.AreEqual(userDTO.CurrentLogbookId, TestHelpersNote.TestLogbook1().Id);
            }
        }
    }
}
