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
    public class UpdateBusinessUnitAsync_Should
    {
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
    }
}
