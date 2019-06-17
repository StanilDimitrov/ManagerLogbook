using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.BusinessUnitServiceTests
{
    [TestClass]
    public class GetAllTownsAsync_Should
    {
        [TestMethod]
        public async Task Should_GetAllTownsAsync()
        {
            var options = TestUtils.GetOptions(nameof(Should_GetAllTownsAsync));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown02());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnits = await sut.GetAllTownsAsync();

                Assert.AreEqual(businessUnits.Count, 2);
            }
        }
    }
}
