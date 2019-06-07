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

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.Towns.AddAsync(TestHelperBusinessUnit.TestTown01());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var businessUnitDTO = await sut.CreateBusinnesUnitAsync("Hilton", "Cerni Vryh 15", "0123456789", "info@hilton.com","Information for BU",1,1);
                                
                mockBusinessValidator.Verify(x => x.IsNameInRange("Hilton"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsAddressInRange("Cerni Vryh 15"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsPhoneNumberValid("0123456789"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsEmailValid("info@hilton.com"), Times.Exactly(1));

                Assert.AreEqual(businessUnitDTO.Name, "Hilton");
                Assert.AreEqual(businessUnitDTO.Address, "Cerni Vryh 15");
                Assert.AreEqual(businessUnitDTO.PhoneNumber, "0123456789");
                Assert.AreEqual(businessUnitDTO.Email, "info@hilton.com");
            }
        }
    }
}
