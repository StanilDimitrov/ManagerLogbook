//using ManagerLogbook.Data;
//using ManagerLogbook.Data.Models;
//using ManagerLogbook.Services;
//using ManagerLogbook.Services.Contracts.Providers;
//using ManagerLogbook.Services.Utils;
//using ManagerLogbook.Tests.HelpersMethods;
//using ManagerLogbook.Tests.Utils;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Threading.Tasks;

//namespace ManagerLogbook.Tests.Services.BusinessUnitServiceTests
//{
//    [TestClass]
//    public class SearchBusinessUnitsAsync_Should
//    {
//        [TestMethod]
//        public async Task Should_SearchBusinessUnitsAsync()
//        {
//            var options = TestUtils.GetOptions(nameof(Should_SearchBusinessUnitsAsync));

//            using (var arrangeContext = new ManagerLogbookContext(options))
//            {
//                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
//                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit02());
//                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
//                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

//                await arrangeContext.SaveChangesAsync();
//            }

//            using (var assertContext = new ManagerLogbookContext(options))
//            {
//                var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);
//                var mockBusinessUnit = new Mock<BusinessUnit>();
              
//                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

//                var businessUnits = await sut.SearchBusinessUnitsAsync("Kempinski", 1, 1);

//                Assert.AreEqual(businessUnits.Count, 1);
//            }
//        }
//    }
//}
