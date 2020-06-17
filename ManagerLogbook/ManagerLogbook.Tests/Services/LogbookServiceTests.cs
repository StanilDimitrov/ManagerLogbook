using AutoFixture;
using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services
{
    [TestClass]
    public class LogbookServiceTests
    {
        #region Members
        private Mock<IUserService> _mockUserService;
        private Mock<IBusinessUnitService> _mockBusinessUnitService;
        private static Fixture _fixture;
        private static DbContextOptions _options;
        private static ManagerLogbookContext _context;
        private static LogbookService _logbookService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockBusinessUnitService = new Mock<IBusinessUnitService>();
            _fixture = new Fixture();
            _options = TestUtils.GetOptions(_fixture.Create<string>());
            _context = new ManagerLogbookContext(_options);
            _logbookService = new LogbookService(_context, _mockBusinessUnitService.Object, _mockUserService.Object);
        }
        #endregion

        #region CreateLogbookAsync
        [TestMethod]
        public async Task CreateLogbookAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            var model = _fixture.Build<LogbookModel>()
               .With(x => x.BusinessUnitId, businessUnit.Id)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {

                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnit.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _logbookService.CreateLogbookAsync(model);
            Assert.IsInstanceOfType(result, typeof(LogbookDTO));
            Assert.AreEqual(1, _context.Logbooks.Count());
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(model.Name, result.Name);

            _mockBusinessUnitService.Verify();
        }

        #endregion

        #region UpdateLogbookAsync
        [TestMethod]
        public async Task UpdateLogbookAsync_Succeed()
        {
            var logbook = _fixture.Build<Logbook>()
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Without(x => x.BusinessUnit)
                .Create();
            var businessUnit = _fixture.Build<BusinessUnit>()
                .Without(x => x.Reviews)
                .Without(x => x.Logbooks)
                .Without(x => x.Users)
                .Without(x => x.CensoredWords)
                .Without(x => x.Town)
                .Without(x => x.BusinessUnitCategory)
                .Create();
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<LogbookModel>()
                .With(x => x.Id, logbook.Id)
                .With(x => x.BusinessUnitId, businessUnit.Id)
                .Create();
            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnit.Id)).ReturnsAsync(businessUnit).Verifiable();
            var result = await _logbookService.UpdateLogbookAsync(model, logbook);
            Assert.IsInstanceOfType(result, typeof(LogbookDTO));
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Picture, result.Picture);

            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region AddManagerToLogbookAsync
        [TestMethod]
        public async Task AddManagerToLogbookAsync_Succeed()
        {
            var logbook = _fixture.Build<Logbook>()
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Without(x => x.BusinessUnit)
                .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                await arrangeContext.SaveChangesAsync();
            }

            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();
            var result = await _logbookService.AddManagerToLogbookAsync(user.Id, logbook.Id);
            Assert.IsInstanceOfType(result, typeof(UserDTO));
            Assert.AreEqual(1, _context.UsersLogbooks.Count());
         
            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task AddManagerToLogbookAsync_ThrowsException_ManagerIsAlreadyInLogbook()
        {
            var logbook = _fixture.Build<Logbook>()
               .Without(x => x.UsersLogbooks)
               .Without(x => x.Notes)
               .Without(x => x.BusinessUnit)
               .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            var userLogbook = _fixture.Build<UsersLogbooks>()
                .With(x => x.UserId, user.Id)
                .With(x => x.LogbookId, logbook.Id)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                arrangeContext.UsersLogbooks.Add(userLogbook);
                await arrangeContext.SaveChangesAsync();
            }

            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();
            var ex = await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => _logbookService.AddManagerToLogbookAsync(user.Id, logbook.Id));
            Assert.AreEqual(ex.Message, string.Format(ServicesConstants.ManagerIsAlreadyInLogbook, user.UserName, logbook.Name));

            _mockUserService.Verify();
        }
        #endregion

        #region  RemoveManagerFromLogbookAsync
        [TestMethod]
        public async Task RemoveManagerFromLogbookAsync_Succeed()
        {
            var logbook = _fixture.Build<Logbook>()
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Without(x => x.BusinessUnit)
                .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            var userLogbook = _fixture.Build<UsersLogbooks>()
                .With(x => x.UserId, user.Id)
                .With(x => x.LogbookId, logbook.Id)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                arrangeContext.UsersLogbooks.Add(userLogbook);
                await arrangeContext.SaveChangesAsync();
            }

            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();

            var result = await _logbookService.RemoveManagerFromLogbookAsync(user.Id, logbook.Id);
            Assert.IsInstanceOfType(result, typeof(UserDTO));
            Assert.AreEqual(0, _context.UsersLogbooks.Count());

            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task RemoveManagerFromLogbookAsync_ThrowsException_ManagerIsNotInLogbook()
        {
            var logbook = _fixture.Build<Logbook>()
               .Without(x => x.UsersLogbooks)
               .Without(x => x.Notes)
               .Without(x => x.BusinessUnit)
               .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                await arrangeContext.SaveChangesAsync();
            }

            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();
            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _logbookService.RemoveManagerFromLogbookAsync(user.Id, logbook.Id));
            Assert.AreEqual(ex.Message, string.Format(ServicesConstants.ManagerIsNotPresentInLogbook, user.UserName, logbook.Name));

            _mockUserService.Verify();
        }
        #endregion

        #region  GetLogbooksByUserAsync
        [TestMethod]
        public async Task GetLogbooksByUserAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
             .Without(x => x.Reviews)
             .Without(x => x.Logbooks)
             .Without(x => x.Users)
             .Without(x => x.CensoredWords)
             .Without(x => x.Town)
             .Without(x => x.BusinessUnitCategory)
             .Create();
            
            var logbook = _fixture.Build<Logbook>()
                .With(x => x.BusinessUnitId, businessUnit.Id)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Without(x => x.BusinessUnit)
                .Create();
            var note = _fixture.Build<Note>()
                .With(x => x.LogbookId, logbook.Id)
                .Without(x => x.Logbook)
                .Without(x => x.NoteCategory)
                .Without(x => x.User)
                .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();
          
            var userLogbook = _fixture.Build<UsersLogbooks>()
                .With(x => x.UserId, user.Id)
                .With(x => x.LogbookId, logbook.Id)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                arrangeContext.Logbooks.Add(logbook);
                arrangeContext.Notes.Add(note);
                arrangeContext.UsersLogbooks.Add(userLogbook);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _logbookService.GetLogbooksByUserAsync(user.Id);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<LogbookDTO>));
            Assert.AreEqual(1, result.Count);

            _mockUserService.Verify();
        }
        #endregion

        #region  AddLogbookToBusinessUnitAsync
        [TestMethod]
        public async Task AddLogbookToBusinessUnitAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
             .Without(x => x.Reviews)
             .Without(x => x.Logbooks)
             .Without(x => x.Users)
             .Without(x => x.CensoredWords)
             .Without(x => x.Town)
             .Without(x => x.BusinessUnitCategory)
             .Create();

            var logbook = _fixture.Build<Logbook>()
                .With(x => x.BusinessUnitId, businessUnit.Id)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Without(x => x.BusinessUnit)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                await arrangeContext.SaveChangesAsync();
            }

             _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnit.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _logbookService.AddLogbookToBusinessUnitAsync(logbook.Id, businessUnit.Id);
            Assert.IsInstanceOfType(result, typeof(LogbookDTO));
            Assert.AreEqual(businessUnit.Id, result.BusinessUnitId);

            _mockUserService.Verify();
        }
        #endregion

        #region CheckIfLogbookNameExist
        [TestMethod]
        public async Task CheckIfLogbookNameExist_Succeed()
        {
            var logbookName = _fixture.Create<string>();

            var result = await _logbookService.CheckIfLogbookNameExist(logbookName);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CheckIfLogbookNameExist_ThrowsException_WhenNameAlreadyExists()
        {
            var logbookName = _fixture.Create<string>();
            var logbook = _fixture.Build<Logbook>()
                   .With(x => x.Name, logbookName)
                   .Without(x => x.BusinessUnit)
                   .Without(x => x.Notes)
                   .Without(x => x.UsersLogbooks)
                   .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<LogbookModel>()
                .With(x => x.Name, logbookName)
                .Create();

            var ex = await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => _logbookService.CheckIfLogbookNameExist(logbookName));
            Assert.AreEqual(ex.Message, string.Format(ServicesConstants.LogbookAlreadyExists, logbookName));
        }
        #endregion
    }
}
