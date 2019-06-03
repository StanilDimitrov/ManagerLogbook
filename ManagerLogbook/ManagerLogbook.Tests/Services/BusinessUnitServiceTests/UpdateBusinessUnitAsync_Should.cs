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
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

                await arrangeContext.BusinessUnits.AddAsync(TestHelperBusinessUnit.TestBusinessUnit01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnit = await sut.UpdateBusinessUnitAsync(TestHelperBusinessUnit.TestBusinessUnit01().Id, "Vitosha", "Cerni Vryh 25", "9876543210", "This is information for BU", "info@vitosha.com", "picturePath");

                mockBusinessValidator.Verify(x => x.IsNameInRange("Vitosha"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsAddressInRange("Cerni Vryh 25"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsPhoneNumberValid("9876543210"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsDescriptionInRange("This is information for BU"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsEmailValid("info@vitosha.com"), Times.Exactly(1));

                Assert.AreEqual(businessUnit.BrandName, "Vitosha");
                Assert.AreEqual(businessUnit.Address, "Cerni Vryh 25");
                Assert.AreEqual(businessUnit.PhoneNumber, "9876543210");
                Assert.AreEqual(businessUnit.Information, "This is information for BU");                
                Assert.AreEqual(businessUnit.Email, "info@vitosha.com");
                Assert.AreEqual(businessUnit.Picture, "picturePath");
            }
        }
    }
}
