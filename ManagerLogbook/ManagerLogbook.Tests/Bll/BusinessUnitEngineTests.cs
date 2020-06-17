using AutoFixture;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Bll;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Bll
{
    [TestClass]
    public class BusinessUnitEngineTests
    {
        #region Members
        private static Fixture _fixture;
        private static Mock<IBusinessUnitService> _mockBusinessUnitService;
        private static Mock<IUserService> _mockUserService;
        private static BusinessUnitEngine _businessUnitEngine;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockBusinessUnitService = new Mock<IBusinessUnitService>();
            _mockUserService = new Mock<IUserService>();
            _businessUnitEngine = new BusinessUnitEngine(_mockUserService.Object, _mockBusinessUnitService.Object);
        }
        #endregion

        #region CreateBusinessUnitAsync
        [TestMethod]
        public async Task CreateBusinessUnitAsync_Succeed()
        {
            var model = _fixture.Create<BusinessUnitModel>();

            await _businessUnitEngine.CreateBusinnesUnitAsync(model);

            _mockBusinessUnitService.Verify(x => x.GetBusinessUnitAsync(model.Id), Times.Once());
            _mockBusinessUnitService.Verify(x => x.CreateBusinnesUnitAsync(model), Times.Once());
        }
        #endregion

        #region UpdateBusinessUnitAsync
        [TestMethod]
        public async Task UpdateBusinessUnitAsync_Succeed_WhenNameIsChanged()
        {
            var model = _fixture.Create<BusinessUnitModel>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(model.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _businessUnitEngine.UpdateBusinessUnitAsync(model);

            _mockBusinessUnitService.Verify(x => x.UpdateBusinessUnitAsync(model, businessUnit), Times.Once());
            _mockBusinessUnitService.Verify(x => x.CheckIfBrandNameExist(model.Name), Times.Once());
            _mockBusinessUnitService.Verify();
        }

        [TestMethod]
        public async Task UpdateBusinessUnitAsync_Succeed_WhenNameIsNotChanged()
        {
            var brandName = _fixture.Create<string>();
            var model = _fixture.Build<BusinessUnitModel>()
                .With(x => x.Name, brandName)
                .Create();

            var businessUnit = _fixture.Build<BusinessUnit>()
                .With(x => x.Name, brandName)
                .Without(x => x.Reviews)
                .Without(x => x.Logbooks)
                .Without(x => x.Users)
                .Without(x => x.CensoredWords)
                .Without(x => x.Town)
                .Without(x => x.BusinessUnitCategory)
                .Create();

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(model.Id)).ReturnsAsync(businessUnit).Verifiable();
            await _businessUnitEngine.UpdateBusinessUnitAsync(model);

            _mockBusinessUnitService.Verify(x => x.UpdateBusinessUnitAsync(model, businessUnit), Times.Once());
            _mockBusinessUnitService.Verify(x => x.CheckIfBrandNameExist(It.IsAny<string>()), Times.Never());
            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region GetLogbooksForBusinessUnitAsync
        [TestMethod]
        public async Task GetLogbooksForBusinessUnitAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();

            var result = await _businessUnitEngine.GetLogbooksForBusinessUnitAsync(businessUnitId);

            _mockBusinessUnitService.Verify(x => x.GetBusinessUnitAsync(businessUnitId), Times.Once());
            _mockBusinessUnitService.Verify(x => x.GetLogbooksForBusinessUnitAsync(businessUnitId), Times.Once());
        }
        #endregion

        #region AddModeratorToBusinessUnitsAsync
        [TestMethod]
        public async Task AddModeratorToBusinessUnitsAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var moderator = _fixture.Build<User>()
                .With(x => x.Id, moderatorId)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.BusinessUnit)
                .Create();

            _mockUserService.Setup(x => x.GetUserAsync(moderatorId)).ReturnsAsync(moderator).Verifiable();

            await _businessUnitEngine.AddModeratorToBusinessUnitsAsync(moderatorId, businessUnitId);

            _mockBusinessUnitService.Verify(x => x.GetBusinessUnitAsync(businessUnitId), Times.Once());
            _mockBusinessUnitService.Verify(x => x.AddModeratorToBusinessUnitsAsync(moderator, businessUnitId), Times.Once());
            _mockUserService.Verify();
        }
        #endregion

        #region RemoveModeratorFromBusinessUnitsAsync
        [TestMethod]
        public async Task RemoveModeratorFromBusinessUnitsAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var moderator = _fixture.Build<User>()
                .With(x => x.Id, moderatorId)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.BusinessUnit)
                .Create();

            _mockUserService.Setup(x => x.GetUserAsync(moderatorId)).ReturnsAsync(moderator).Verifiable();

            await _businessUnitEngine.RemoveModeratorFromBusinessUnitsAsync(moderatorId, businessUnitId);

            _mockBusinessUnitService.Verify(x => x.GetBusinessUnitAsync(businessUnitId), Times.Once());
            _mockBusinessUnitService.Verify(x => x.RemoveModeratorFromBusinessUnitsAsync(moderator, businessUnitId), Times.Once());
            _mockUserService.Verify();
        }
        #endregion
    }
}
