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
            var businessUnitDto = _fixture.Create<BusinessUnitDTO>();
            var model = _fixture.Create<BusinessUnitModel>();

            _mockBusinessUnitService.Setup(x => x.CheckIfBrandNameExist(model.Name)).ReturnsAsync(false).Verifiable();
            _mockBusinessUnitService.Setup(x => x.CreateBusinnesUnitAsync(model)).ReturnsAsync(businessUnitDto).Verifiable();

            var result = await _businessUnitEngine.CreateBusinnesUnitAsync(model);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));

            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region UpdateBusinessUnitAsync
        [TestMethod]
        public async Task UpdateBusinessUnitAsync_Succeed_WhenNameIsChanged()
        {
            var businessUnitDto = _fixture.Create<BusinessUnitDTO>();
            var model = _fixture.Create<BusinessUnitModel>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            _mockBusinessUnitService.Setup(x => x.UpdateBusinessUnitAsync(model, businessUnit)).ReturnsAsync(businessUnitDto).Verifiable();
            _mockBusinessUnitService.Setup(x => x.CheckIfBrandNameExist(model.Name)).ReturnsAsync(false).Verifiable();
            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(model.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _businessUnitEngine.UpdateBusinessUnitAsync(model);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));

            _mockBusinessUnitService.Verify();
        }

        [TestMethod]
        public async Task UpdateBusinessUnitAsync_Succeed_WhenNameIsNotChanged()
        {
            var businessUnitDto = _fixture.Create<BusinessUnitDTO>();
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

            _mockBusinessUnitService.Setup(x => x.UpdateBusinessUnitAsync(model, businessUnit)).ReturnsAsync(businessUnitDto).Verifiable();
            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(model.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _businessUnitEngine.UpdateBusinessUnitAsync(model);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));

            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region GetLogbooksForBusinessUnitAsync
        [TestMethod]
        public async Task GetLogbooksForBusinessUnitAsync_Succeed()
        {
            var businessUnitDto = _fixture.Create<BusinessUnitDTO>();
            var businessUnitId = _fixture.Create<int>();
            var logbookDTOs = new List<LogbookDTO>()
            {
                _fixture.Build<LogbookDTO>()
                .Without(x => x.Notes)
                .Create(),
                _fixture.Build<LogbookDTO>()
                .Without(x => x.Notes)
                .Create()
            };

            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnitId)).ReturnsAsync(businessUnit).Verifiable();
            _mockBusinessUnitService.Setup(x => x.GetLogbooksForBusinessUnitAsync(businessUnitId)).ReturnsAsync(logbookDTOs).Verifiable();

            var result = await _businessUnitEngine.GetLogbooksForBusinessUnitAsync(businessUnitId);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<LogbookDTO>));

            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region AddModeratorToBusinessUnitsAsync
        [TestMethod]
        public async Task AddModeratorToBusinessUnitsAsync_Succeed()
        {
            var moderatorDto = _fixture.Create<UserDTO>();
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var businessUnit = _fixture.Build<BusinessUnit>()
                .With(x => x.Id, businessUnitId)
                .Without(x => x.Reviews)
                .Without(x => x.Logbooks)
                .Without(x => x.Users)
                .Without(x => x.CensoredWords)
                .Without(x => x.Town)
                .Without(x => x.BusinessUnitCategory)
                 .Create();

            var moderator = _fixture.Build<User>()
                .With(x => x.Id, moderatorId)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.BusinessUnit)
                .Create();

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnitId)).ReturnsAsync(businessUnit).Verifiable();
            _mockBusinessUnitService.Setup(x => x.AddModeratorToBusinessUnitsAsync(moderator, businessUnitId)).ReturnsAsync(moderatorDto).Verifiable();
            _mockUserService.Setup(x => x.GetUserAsync(moderatorId)).ReturnsAsync(moderator).Verifiable();

            var result = await _businessUnitEngine.AddModeratorToBusinessUnitsAsync(moderatorId, businessUnitId);
            Assert.IsInstanceOfType(result,typeof(UserDTO));

            _mockBusinessUnitService.Verify();
            _mockUserService.Verify();
        }
        #endregion

        #region RemoveModeratorFromBusinessUnitsAsync
        [TestMethod]
        public async Task RemoveModeratorFromBusinessUnitsAsync_Succeed()
        {
            var moderatorDto = _fixture.Create<UserDTO>();
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var businessUnit = _fixture.Build<BusinessUnit>()
                .With(x => x.Id, businessUnitId)
                .Without(x => x.Reviews)
                .Without(x => x.Logbooks)
                .Without(x => x.Users)
                .Without(x => x.CensoredWords)
                .Without(x => x.Town)
                .Without(x => x.BusinessUnitCategory)
                 .Create();

            var moderator = _fixture.Build<User>()
                .With(x => x.Id, moderatorId)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.BusinessUnit)
                .Create();

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnitId)).ReturnsAsync(businessUnit).Verifiable();
            _mockBusinessUnitService.Setup(x => x.RemoveModeratorFromBusinessUnitsAsync(moderator, businessUnitId)).ReturnsAsync(moderatorDto).Verifiable();
            _mockUserService.Setup(x => x.GetUserAsync(moderatorId)).ReturnsAsync(moderator).Verifiable();

            var result = await _businessUnitEngine.RemoveModeratorFromBusinessUnitsAsync(moderatorId, businessUnitId);
            Assert.IsInstanceOfType(result, typeof(UserDTO));

            _mockBusinessUnitService.Verify();
            _mockUserService.Verify();
        }
        #endregion
    }
}
