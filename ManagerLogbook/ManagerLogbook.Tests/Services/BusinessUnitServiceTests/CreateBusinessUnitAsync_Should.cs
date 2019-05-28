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
    public class CreateBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnCreateBusinessUnit()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnCreateBusinessUnit));

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
            }
        }
    }
}
