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
    public class CreateBusinessUnitCategoryAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnCreateBusinessUnitCategory()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnCreateBusinessUnitCategory));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitCategoryDTO = await sut.CreateBusinessUnitCategoryAsync("Restaurant");

                mockBusinessValidator.Verify(x => x.IsNameInRange("Restaurant"), Times.Exactly(1));
                
                Assert.AreEqual(businessUnitCategoryDTO.Name, "Restaurant");                
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitCategoryNameAlreadyExists()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitCategoryNameAlreadyExists));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit02());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => sut.CreateBusinessUnitCategoryAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01().Name));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitCategoryNameAlreadyExists));
            }
        }
    }
}
