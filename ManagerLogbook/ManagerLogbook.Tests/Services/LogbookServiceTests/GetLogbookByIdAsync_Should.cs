using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.LogbookServiceTests
{
    [TestClass]
    public class GetLogbookByIdAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetLogbookById()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetLogbookById));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new LogbookService(assertContext, mockBusinessValidator.Object);

                var getLogbookById = await sut.GetLogbookById(1);

                Assert.AreEqual(getLogbookById.Id, 1);
            }
        }
    }
}
