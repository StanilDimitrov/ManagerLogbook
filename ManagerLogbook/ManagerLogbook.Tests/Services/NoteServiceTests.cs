using AutoFixture;
using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services
{
    [TestClass]
    public class NoteServiceTests
    {
        #region Members
        private static Fixture _fixture;
        private static DbContextOptions _options;
        private static ManagerLogbookContext _context;
        private static NoteService _noteService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _options = TestUtils.GetOptions(_fixture.Create<string>());
            _context = new ManagerLogbookContext(_options);
            _noteService = new NoteService(_context);
        }
        #endregion

        #region CreateNoteAsync
        [TestMethod]
        public async Task CreateNoteAsync_Succeed()
        {
            var logbookId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();

            var model = _fixture.Create<NoteModel>();

            var result = await _noteService.CreateNoteAsync(model, logbookId, userId);

            Assert.IsInstanceOfType(result, typeof(NoteDTO));
            Assert.AreEqual(1, _context.Notes.Count());
            Assert.AreEqual(1, result.Id);
        }
        #endregion

        #region UpdateNoteAsync
        [TestMethod]
        public async Task UpdateNoteAsync_Succeed_WhenCategoryIsNotNull()
        {
            var note = _fixture.Build<Note>()
                .Without(x => x.NoteCategory)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Create<NoteModel>();

            var result = await _noteService.UpdateNoteAsync(model, note);

            Assert.IsInstanceOfType(result, typeof(NoteDTO));
            Assert.AreEqual(model.Description, result.Description);
            Assert.AreEqual(model.Image, result.Image);
            Assert.AreEqual(model.CategoryId, result.CategoryId);
            Assert.AreEqual(model.IsActiveTask, result.IsActiveTask);
        }

        [TestMethod]
        public async Task UpdateNoteAsync_Succeed_WhenCategoryIsNull()
        {
            var note = _fixture.Build<Note>()
                .Without(x => x.NoteCategory)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<NoteModel>()
                .Without(x => x.CategoryId)
                .Create();

            var result = await _noteService.UpdateNoteAsync(model, note);

            Assert.IsInstanceOfType(result, typeof(NoteDTO));
            Assert.AreEqual(model.Description, result.Description);
            Assert.AreEqual(model.Image, result.Image);
            Assert.AreEqual(note.NoteCategoryId, result.CategoryId);
            Assert.AreEqual(note.IsActiveTask, result.IsActiveTask);
        }
        #endregion

        #region DeactivateNoteAsync
        [TestMethod]
        public async Task DeactivateNoteAsync_Succeed()
        {
            var note = _fixture.Build<Note>()
                .Without(x => x.NoteCategory)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var isActiveStatus = _fixture.Create<bool>();

            var result = await _noteService.DeactivateNoteActiveStatus(note, isActiveStatus);

            Assert.IsInstanceOfType(result, typeof(NoteDTO));
            Assert.AreEqual(isActiveStatus, result.IsActiveTask);
        }
        #endregion

        #region ShowLogbookNotesWithActiveStatusAsync
        [TestMethod]
        public async Task ShowLogbookNotesWithActiveStatusAsync_Succeed()
        {
            var logbook = _fixture.Build<Logbook>()
              .Without(x => x.BusinessUnit)
              .Without(x => x.Notes)
              .Without(x => x.UsersLogbooks)
              .Create();

            var user = _fixture.Build<User>()
              .Without(x => x.BusinessUnit)
              .Without(x => x.Notes)
              .Without(x => x.UsersLogbooks)
              .Create();

            var noteCategory = _fixture.Build<NoteCategory>()
             .Without(x => x.Notes)
             .Create();

            var note = _fixture.Build<Note>()
                .With(x => x.LogbookId, logbook.Id)
                .With(x => x.NoteCategoryId, noteCategory.Id)
                .With(x => x.UserId, user.Id)
                .With(x => x.IsActiveTask, true)
                .Without(x => x.NoteCategory)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            var userLogbooks = _fixture.Build<UsersLogbooks>()
               .With(x => x.LogbookId, logbook.Id)
               .With(x => x.UserId, user.Id)
               .Without(x => x.Logbook)
               .Without(x => x.User)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                arrangeContext.Users.Add(user);
                arrangeContext.NoteCategories.Add(noteCategory);
                arrangeContext.UsersLogbooks.Add(userLogbooks);
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _noteService.ShowLogbookNotesWithActiveStatusAsync(logbook.Id);

            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<NoteDTO>));
            Assert.AreEqual(1, result.Count);
        }
        #endregion

        #region ShowLogbookNotesAsync
        [TestMethod]
        public async Task ShowLogbookNotesAsync_Succeed()
        {
            var logbook = _fixture.Build<Logbook>()
              .Without(x => x.BusinessUnit)
              .Without(x => x.Notes)
              .Without(x => x.UsersLogbooks)
              .Create();

            var user = _fixture.Build<User>()
              .Without(x => x.BusinessUnit)
              .Without(x => x.Notes)
              .Without(x => x.UsersLogbooks)
              .Create();

            var noteCategory = _fixture.Build<NoteCategory>()
             .Without(x => x.Notes)
             .Create();

            var note = _fixture.Build<Note>()
                .With(x => x.LogbookId, logbook.Id)
                .With(x => x.NoteCategoryId, noteCategory.Id)
                .With(x => x.UserId, user.Id)
                .Without(x => x.NoteCategory)
                .Without(x => x.Logbook)
                .Without(x => x.User)
                .Create();

            var userLogbooks = _fixture.Build<UsersLogbooks>()
               .With(x => x.LogbookId, logbook.Id)
               .With(x => x.UserId, user.Id)
               .Without(x => x.Logbook)
               .Without(x => x.User)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Logbooks.Add(logbook);
                arrangeContext.Users.Add(user);
                arrangeContext.NoteCategories.Add(noteCategory);
                arrangeContext.UsersLogbooks.Add(userLogbooks);
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _noteService.ShowLogbookNotesWithActiveStatusAsync(logbook.Id);

            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<NoteDTO>));
            Assert.AreEqual(1, result.Count);
        }
        #endregion

        #region CheckIfUserIsAuthorized
        [TestMethod]
        public async Task CheckIfUserIsAuthorized_Succeed()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();

            var userLogbooks = _fixture.Build<UsersLogbooks>()
               .With(x => x.LogbookId, logbookId)
               .With(x => x.UserId, userId)
               .Without(x => x.Logbook)
               .Without(x => x.User)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.UsersLogbooks.Add(userLogbooks);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _noteService.CheckIfUserIsAuthorized(userId, logbookId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CheckIfLogbookNameExist_ThrowsException_WhenUserIsNotAuthorized()
        {
            var userId = _fixture.Create<string>();
            var logbookId = _fixture.Create<int>();

            var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => _noteService.CheckIfUserIsAuthorized(userId, logbookId));
            Assert.AreEqual(ServicesConstants.UserIsNotAuthorizedToViewNotes, ex.Message);
        }
        #endregion

        #region GetNoteAsync
        [TestMethod]
        public async Task GetNoteAsync_Succeed()
        {
            var noteId = _fixture.Create<int>();

            var note = _fixture.Build<Note>()
               .With(x => x.Id, noteId)
               .Without(x => x.NoteCategory)
               .Without(x => x.Logbook)
               .Without(x => x.User)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Notes.Add(note);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _noteService.GetNoteAsync(noteId);

            Assert.IsInstanceOfType(result, typeof(Note));
            Assert.AreEqual(noteId, result.Id);
        }

        [TestMethod]
        public async Task GetNoteAsync_ThrowsException_WhenNoteNotFound()
        {
            var noteId = _fixture.Create<int>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _noteService.GetNoteAsync(noteId));
            Assert.AreEqual(ServicesConstants.NoteNotFound, ex.Message);
        }
        #endregion

        #region GetNoteCategoryAsync
        [TestMethod]
        public async Task GetNoteCategoryAsync_Succeed()
        {
            var noteCategoryId = _fixture.Create<int>();

            var noteCategory = _fixture.Build<NoteCategory>()
               .With(x => x.Id, noteCategoryId)
               .Without(x => x.Notes)
               .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.NoteCategories.Add(noteCategory);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _noteService.GetNoteCategoryAsync(noteCategoryId);

            Assert.IsInstanceOfType(result, typeof(NoteCategory));
            Assert.AreEqual(noteCategoryId, result.Id);
        }

        [TestMethod]
        public async Task GetNoteCategoryAsync_ThrowsException_WhenCategoryNotFound()
        {
            var noteCategoryId = _fixture.Create<int>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _noteService.GetNoteCategoryAsync(noteCategoryId));
            Assert.AreEqual(ServicesConstants.NoteCategoryDoesNotExists, ex.Message);
        }
        #endregion
    }
}
