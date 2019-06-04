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
    public class GetAllLogbooksByUserAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnAllLogbooksByUser()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnAllLogbooksByUser));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.Notes.AddAsync(TestHelpersLogbook.TestNote02());

                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook03());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks03());
                               
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var allLogbooksByUser = await sut.GetAllLogbooksByUserAsync(TestHelpersLogbook.TestUser01().Id);
                
                Assert.AreEqual(allLogbooksByUser.Count,2);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenUserWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnAllLogbooksByUser));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.Notes.AddAsync(TestHelpersLogbook.TestNote02());

                await arrangeContext.Users.AddAsync(TestHelpersLogbook.TestUser01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook03());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks01());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersLogbook.TestUsersLogbooks03());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetAllLogbooksByUserAsync("2"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotFound));
            }
        }
    }
}
