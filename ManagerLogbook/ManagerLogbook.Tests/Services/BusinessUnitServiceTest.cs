using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services
{
    public class BusinessUnitServiceTest
    {
        [TestClass]
        public class CreateBusinessUnitAsync_Should
        {
            [TestMethod]
            public async Task Succeed_ReturnBusinessUnit()
            {
                var options = TestUtils.GetOptions(nameof(Succeed_ReturnBusinessUnit));

                using (var assertContext = new ManagerLogbookContext(options))
                {
                    var mockBusinessValidator = new Mock<IBusinessValidator>();

                    var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                    var businessUnit = await sut.CreateBusinnesUnitAsync("Hilton", "Cerni Vryh 15", "0123456789", "info@hilton.com");

                    mockBusinessValidator.Verify(x => x.IsNameInRange("Hilton"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsAddressInRange("Cerni Vryh 15"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsPhoneNumberValid("0123456789"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsEmailValid("info@hilton.com"), Times.Exactly(1));

                    Assert.AreEqual(businessUnit.Name, "Hilton");
                    Assert.AreEqual(businessUnit.Address, "Cerni Vryh 15");
                    Assert.AreEqual(businessUnit.PhoneNumber, "0123456789");
                    Assert.AreEqual(businessUnit.Email, "info@hilton.com");
                    ;
                }
            }

            [TestMethod]
            public async Task Succeed_ReturnIsBusinessUnitExists()
            {
                var options = TestUtils.GetOptions(nameof(Succeed_ReturnIsBusinessUnitExists));

                using (var arrangeContext = new ManagerLogbookContext(options))
                {
                    await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                    await arrangeContext.SaveChangesAsync();
                }

                using (var assertContext = new ManagerLogbookContext(options))
                {
                    var mockBusinessValidator = new Mock<IBusinessValidator>();

                    var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                    var isBusinessUnitExists = await sut.IsBusinessUnitExists("Hilton");

                    Assert.AreEqual(isBusinessUnitExists.Name, "Hilton");
                }
            }

            [TestMethod]
            public async Task Succeed_UpdateBusinessUnit()
            {
                var options = TestUtils.GetOptions(nameof(Succeed_UpdateBusinessUnit));

                using (var arrangeContext = new ManagerLogbookContext(options))
                {
                    await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                    await arrangeContext.SaveChangesAsync();
                }

                using (var assertContext = new ManagerLogbookContext(options))
                {
                    var mockBusinessValidator = new Mock<IBusinessValidator>();

                    var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                    var businessUnit = await sut.UpdateBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit01(), "Kempinski", "Cerni Vryh 25", "9876543210", "info@kempinski.com", "picturePath");

                    mockBusinessValidator.Verify(x => x.IsNameInRange("Kempinski"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsAddressInRange("Cerni Vryh 25"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsPhoneNumberValid("9876543210"), Times.Exactly(1));
                    mockBusinessValidator.Verify(x => x.IsEmailValid("info@kempinski.com"), Times.Exactly(1));

                    Assert.AreEqual(businessUnit.Name, "Kempinski");
                    Assert.AreEqual(businessUnit.Address, "Cerni Vryh 25");
                    Assert.AreEqual(businessUnit.PhoneNumber, "9876543210");
                    Assert.AreEqual(businessUnit.Email, "info@kempinski.com");
                    Assert.AreEqual(businessUnit.Picture, "picturePath");
                }
            }

            [TestMethod]
            public async Task Should_AddLogbookToBusinessUnitAsync()
            {
                var options = TestUtils.GetOptions(nameof(Should_AddLogbookToBusinessUnitAsync));

                using (var arrangeContext = new ManagerLogbookContext(options))
                {
                    await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                    await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook01());

                    await arrangeContext.SaveChangesAsync();
                }

                using (var assertContext = new ManagerLogbookContext(options))
                {
                    var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                    var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                    var businessUnit = await sut.AddLogbookToBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit01().Id, TestHelperBusinessUnit.TestLogbook01().Id);

                    Assert.AreEqual(businessUnit.Logbooks.Count, 1);
                }
            }

            [TestMethod]
            public async Task Should_FindAllLogbooksForBusinessUnitAsync()
            {
                var options = TestUtils.GetOptions(nameof(Should_FindAllLogbooksForBusinessUnitAsync));

                using (var arrangeContext = new ManagerLogbookContext(options))
                {
                    await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                    await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook02());
                    await arrangeContext.Logbooks.AddAsync(TestHelperBusinessUnit.TestLogbook03());

                    await arrangeContext.SaveChangesAsync();
                }

                using (var assertContext = new ManagerLogbookContext(options))
                {
                    var mockBusinessValidator = new Mock<IBusinessValidator>(MockBehavior.Strict);

                    var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                    var logbooks = await sut.FindAllLogbooksForBusinessUnitAsync(1);

                    Assert.AreEqual(logbooks.Count, 2);
                }
            }
        }
    }
}
