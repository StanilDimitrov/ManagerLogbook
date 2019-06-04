using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.LogbookServiceTests
{
    [TestClass]
    public class UpdateLogbookAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnLogbookWhenParametersAreValid()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnLogbookWhenParametersAreValid));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.Notes.AddAsync(TestHelpersLogbook.TestNote01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var logbookDTO = await sut.UpdateLogbookAsync(1, "Hotel", "picture");

                Assert.AreEqual(logbookDTO.Name, "Hotel");
                Assert.AreEqual(logbookDTO.Picture, "picture");
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenNameIsNullOrEmpty()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenNameIsNullOrEmpty));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.Notes.AddAsync(TestHelpersLogbook.TestNote01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateLogbookAsync(1, null, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.NameCanNotBeNullOrEmpty));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookNameWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenNameIsNullOrEmpty));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Logbooks.AddAsync(TestHelpersLogbook.TestLogbook01());
                await arrangeContext.BusinessUnits.AddAsync(TestHelpersLogbook.TestBusinessUnit01());
                await arrangeContext.Notes.AddAsync(TestHelpersLogbook.TestNote01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedBusinessValidator = new Mock<IBusinessValidator>();
                var sut = new LogbookService(assertContext, mockedBusinessValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.UpdateLogbookAsync(2, null, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }
    }
}
