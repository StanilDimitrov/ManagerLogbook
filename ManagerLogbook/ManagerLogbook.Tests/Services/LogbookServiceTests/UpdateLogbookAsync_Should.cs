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

                var logbookDTO = await sut.UpdateLogbookAsync(1, "Hotel", 1,"picture");

                Assert.AreEqual(logbookDTO.Name, "Hotel");
                Assert.AreEqual(logbookDTO.Picture, "picture");
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenNameIsNull()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenNameIsNull));

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

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateLogbookAsync(1, null, 1, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.NameCanNotBeNullOrEmpty));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookIsNull()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookIsNull));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.UpdateLogbookAsync(10, "name", 1, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenBusinessUnitIsNull()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenBusinessUnitIsNull));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.UpdateLogbookAsync(1, "name", 100, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }


        [TestMethod]
        public async Task ThrowsExeptionWhenNameIsEmpty()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenNameIsEmpty));

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

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateLogbookAsync(1, string.Empty, 1, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.NameCanNotBeNullOrEmpty));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookNameWasNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookNameWasNotFound));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.UpdateLogbookAsync(2, null, 1, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenLogbookNameAlreadyExists()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenLogbookNameAlreadyExists));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.UpdateLogbookAsync(2, TestHelpersLogbook.TestLogbook01().Name,1, "picture"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookNotFound));
            }
        }
    }
}
