using AutoFixture;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Bll;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Bll
{
    [TestClass]
    public class LogbookEngineTests
    {
        #region Members
        private static Fixture _fixture;
        private static Mock<ILogbookService> _mockLogbookService;
        private static Mock<IBusinessUnitService> _mockBusinessUnitService;
        private static Mock<IUserService> _mockUserService;
        private static LogbookEngine _logbookEngine;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockLogbookService = new Mock<ILogbookService>();
            _mockBusinessUnitService = new Mock<IBusinessUnitService>();
            _mockUserService = new Mock<IUserService>();
            _logbookEngine = new LogbookEngine(_mockLogbookService.Object, _mockBusinessUnitService.Object, _mockUserService.Object);
        }
        #endregion

        #region CreateLogbookAsync
        [TestMethod]
        public async Task CreateLogbookAsync_Succeed()
        {
            var model = _fixture.Create<LogbookModel>();

            var result = await _logbookEngine.CreateLogbookAsync(model);

            _mockLogbookService.Verify(x => x.CreateLogbookAsync(model), Times.Once());
            _mockLogbookService.Verify(x => x.CheckIfLogbookNameExist(model.Name), Times.Once());
            _mockBusinessUnitService.Verify(x => x.GetBusinessUnitAsync(model.BusinessUnitId.Value), Times.Once());
            
        }
        #endregion

        #region UpdateLogbookAsync
        [TestMethod]
        public async Task UpdateLogbookAsync_Succeed_WhenNameIsChanged()
        {
            var logbook = _fixture.Build<Logbook>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Create();

            var model = _fixture.Create<LogbookModel>();

            _mockLogbookService.Setup(x => x.GetLogbookAsync(model.Id)).ReturnsAsync(logbook).Verifiable();

            var result = await _logbookEngine.UpdateLogbookAsync(model);
            _mockLogbookService.Verify(x => x.CheckIfLogbookNameExist(model.Name), Times.Once());
            _mockLogbookService.Verify(x => x.UpdateLogbookAsync(model, logbook), Times.Once());
            _mockLogbookService.Verify();
        }

        [TestMethod]
        public async Task UpdateLogbookAsync_Succeed_WhenNameIsNotChanged()
        {
            var logbookName = _fixture.Create<string>();
            var logbook = _fixture.Build<Logbook>()
                .With(x => x.Name, logbookName)
                .Without(x => x.BusinessUnit)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Create();

            var model = _fixture.Build<LogbookModel>()
                .With(x => x.Name, logbookName)
                .Create();

            _mockLogbookService.Setup(x => x.GetLogbookAsync(model.Id)).ReturnsAsync(logbook).Verifiable();

            var result = await _logbookEngine.UpdateLogbookAsync(model);

            _mockLogbookService.Verify(x => x.CheckIfLogbookNameExist(It.IsAny<string>()), Times.Never());
            _mockLogbookService.Verify(x => x.UpdateLogbookAsync(model, logbook), Times.Once());

            _mockLogbookService.Verify();
        }
        #endregion

    }
}
