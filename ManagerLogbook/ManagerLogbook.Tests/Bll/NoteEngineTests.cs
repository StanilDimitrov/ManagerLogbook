using AutoFixture;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Bll;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Bll
{
    [TestClass]
    public class NoteEngineTests
    {
        #region Members
        private static Fixture _fixture;
        private static Mock<INoteService> _mockNoteService;
        private static Mock<IUserService> _mockUserService;
        private static NoteEngine _noteEngine;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockNoteService = new Mock<INoteService>();
            _mockUserService = new Mock<IUserService>();
            _noteEngine = new NoteEngine(_mockNoteService.Object, _mockUserService.Object);
        }
        #endregion

        #region CreateNoteAsync
        [TestMethod]
        public async Task CreateNoteAsync_Succeed_WhenCategoryIdIsNotNullAndCategoryIsTask()
        {
            var logbookId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();
            var model = _fixture.Build<NoteModel>()
                .With(x => x.IsActiveTask, false)
                .Create();
            var noteCategory = _fixture.Build<NoteCategory>()
                .With(x => x.Name, "Task")
                .Without(x => x.Notes)
                .Create();

            _mockNoteService.Setup(x => x.GetNoteCategoryAsync(model.CategoryId.Value)).ReturnsAsync(noteCategory).Verifiable();

            await _noteEngine.CreateNoteAsync(model, logbookId, userId);
            Assert.IsTrue(model.IsActiveTask);

            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.CreateNoteAsync(model, logbookId, userId), Times.Once());

            _mockNoteService.Verify();
        }

        [TestMethod]
        public async Task CreateNoteAsync_Succeed_WhenCategoryIdIsNullAndCategoryIsNotTask()
        {
            var logbookId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();
            var model = _fixture.Build<NoteModel>()
                .Without(x => x.CategoryId)
                .Create();

            await _noteEngine.CreateNoteAsync(model, logbookId, userId);
            
            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.GetNoteCategoryAsync(It.IsAny<int>()), Times.Never());
            _mockNoteService.Verify(x => x.CreateNoteAsync(model, logbookId, userId), Times.Once());
        }
        #endregion

        #region UpdateNoteAsync
        [TestMethod]
        public async Task UpdateNoteAsync_Succeed_WhenCategoryIdIsNull()
        {
            var userId = _fixture.Create<string>();
            var model = _fixture.Build<NoteModel>()
                .With(x => x.UserId, userId)
                .Without(x => x.CategoryId)
                .Create();

            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            var note = _fixture.Build<Note>()
                .With(x => x.UserId, userId)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Without(x => x.NoteCategory)
                .Create();

            _mockNoteService.Setup(x => x.GetNoteAsync(model.Id)).ReturnsAsync(note).Verifiable();
            _mockUserService.Setup(x => x.GetUserAsync(model.UserId)).ReturnsAsync(user).Verifiable();

            await _noteEngine.UpdateNoteAsync(model);

            _mockNoteService.Verify(x => x.UpdateNoteAsync(model,note), Times.Once());
            _mockNoteService.Verify(x => x.GetNoteCategoryAsync(It.IsAny<int>()), Times.Never());

            _mockNoteService.Verify();
            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task UpdateNoteAsync_Succeed_WhenCategoryIdIsNotNullAndCategoryNameIsTask()
        {
            var userId = _fixture.Create<string>();
            var model = _fixture.Build<NoteModel>()
                .With(x => x.UserId, userId)
                .With(x => x.IsActiveTask, false)
                .Create();

            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            var note = _fixture.Build<Note>()
                .With(x => x.UserId, userId)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Without(x => x.NoteCategory)
                .Create();

            var noteCategory = _fixture.Build<NoteCategory>()
                .With(x => x.Name, "Task")
                .Without(x => x.Notes)
                .Create();

            _mockNoteService.Setup(x => x.GetNoteAsync(model.Id)).ReturnsAsync(note).Verifiable();
            _mockNoteService.Setup(x => x.GetNoteCategoryAsync(model.CategoryId.Value)).ReturnsAsync(noteCategory).Verifiable();
            _mockUserService.Setup(x => x.GetUserAsync(model.UserId)).ReturnsAsync(user).Verifiable();

            await _noteEngine.UpdateNoteAsync(model);
            Assert.IsTrue(model.IsActiveTask);

            _mockNoteService.Verify(x => x.UpdateNoteAsync(model, note), Times.Once());
            _mockNoteService.Verify(x => x.GetNoteCategoryAsync(model.CategoryId.Value), Times.Once());

            _mockNoteService.Verify();
            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task UpdateNoteAsync_ThrowsExceptionWhenUserIsNotAuthorized()
        {
            var model = _fixture.Build<NoteModel>()
                .Create();

            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.Notes)
                .Without(x => x.UsersLogbooks)
                .Create();

            var note = _fixture.Build<Note>()
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Without(x => x.NoteCategory)
                .Create();

            _mockNoteService.Setup(x => x.GetNoteAsync(model.Id)).ReturnsAsync(note).Verifiable();
            _mockUserService.Setup(x => x.GetUserAsync(model.UserId)).ReturnsAsync(user).Verifiable();

            var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => _noteEngine.UpdateNoteAsync(model));
            Assert.AreEqual(string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote, user.UserName), ex.Message);
        }

        #endregion

        #region DeactivateNoteActiveStatus
        [TestMethod]
        public async Task DeactivateNoteActiveStatus_Succeed()
        {
            var logbookId = _fixture.Create<int>();
            var noteId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();

            var user = _fixture.Build<User>()
               .With(x => x.Id, userId)
               .Without(x => x.BusinessUnit)
               .Without(x => x.Notes)
               .Without(x => x.UsersLogbooks)
               .Create();

            var note = _fixture.Build<Note>()
                .With(x => x.LogbookId, logbookId)
                .With(x => x.Id, noteId)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Without(x => x.NoteCategory)
                .Create();

            _mockNoteService.Setup(x => x.GetNoteAsync(noteId)).ReturnsAsync(note).Verifiable();

            await _noteEngine.DeactivateNoteActiveStatus(noteId, userId);

            _mockNoteService.Verify(x => x.DeactivateNoteActiveStatus(note, false), Times.Once());

            _mockNoteService.Verify();
        }
        #endregion

        #region ShowLogbookNotesForDaysBeforeAsync
        [TestMethod]
        public async Task ShowLogbookNotesForDaysBeforeAsync_Succeed()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();
            var days = _fixture.Create<int>();

            await _noteEngine.ShowLogbookNotesForDaysBeforeAsync(userId, logbookId, days);

            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.ShowLogbookNotesForDaysBeforeAsync(logbookId, days), Times.Once());
        }
        #endregion

        #region ShowLogbookNotesWithActiveStatusAsync
        [TestMethod]
        public async Task ShowLogbookNotesWithActiveStatusAsync_Succeed()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();

            await _noteEngine.ShowLogbookNotesWithActiveStatusAsync(userId, logbookId);

            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.ShowLogbookNotesWithActiveStatusAsync(logbookId), Times.Once());
        }
        #endregion

        #region ShowLogbookNotesAsync
        [TestMethod]
        public async Task ShowLogbookNotesAsync_Succeed()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();

            await _noteEngine.ShowLogbookNotesAsync(userId, logbookId);

            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.ShowLogbookNotesAsync(logbookId), Times.Once());
        }
        #endregion

        #region SearchNotesAsync
        [TestMethod]
        public async Task SearchNotesAsync_Succeed()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();
            var model = _fixture.Create<SearchNoteModel>();

            await _noteEngine.SearchNotesAsync(userId, logbookId, model);

            _mockNoteService.Verify(x => x.CheckIfUserIsAuthorized(userId, logbookId), Times.Once());
            _mockNoteService.Verify(x => x.SearchNotesAsync(userId, logbookId, model), Times.Once());
        }
        #endregion
    }
}
