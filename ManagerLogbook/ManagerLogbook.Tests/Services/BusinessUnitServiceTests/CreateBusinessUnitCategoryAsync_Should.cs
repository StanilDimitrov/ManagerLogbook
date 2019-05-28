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

                var businessUnitCategory = await sut.CreateBusinessUnitCategoryAsync("Restaurant");

                mockBusinessValidator.Verify(x => x.IsNameInRange("Restaurant"), Times.Exactly(1));
                
                Assert.AreEqual(businessUnitCategory.Name, "Restaurant");                
            }
        }
    }
}
