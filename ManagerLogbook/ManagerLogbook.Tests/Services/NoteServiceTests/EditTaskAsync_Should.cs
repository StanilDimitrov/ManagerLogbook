using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.NoteServiceTests
{
    [TestClass]
    public class EditTaskAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeption()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser2());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.EditNoteAsync(TestHelpersNote.TestNote1(),
                                                     TestHelpersNote.TestUser2().Id, null, null, null));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote));
            }
        }

        [TestMethod]
        public async Task SuccessfullyEditNote()
        {
            var options = TestUtils.GetOptions(nameof(SuccessfullyEditNote));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var note = await sut.EditNoteAsync(TestHelpersNote.TestNote1(),
                                                    TestHelpersNote.TestUser1().Id,
                                                   "The room is clean", "710f8fd0-c90f-4d6a-b421-3aef173d9cf4", TestHelpersNote.TestNoteCategory1().Id);

                Assert.AreEqual(note.Description, "The room is clean");
                Assert.AreEqual(note.Image, "710f8fd0-c90f-4d6a-b421-3aef173d9cf4");
                Assert.AreEqual(note.NoteCategoryId, TestHelpersNote.TestNoteCategory1().Id);
            }
        }

        [TestMethod]
        public async Task SuccessfullyEditNoteWithCategoryFromTypeTask()
        {
            var options = TestUtils.GetOptions(nameof(SuccessfullyEditNoteWithCategoryFromTypeTask));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory2());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var note = await sut.EditNoteAsync(TestHelpersNote.TestNote1(),
                                                    TestHelpersNote.TestUser1().Id,
                                                   "The room is clean", "710f8fd0-c90f-4d6a-b421-3aef173d9cf4", TestHelpersNote.TestNoteCategory2().Id);

                Assert.AreEqual(note.Description, "The room is clean");
                Assert.AreEqual(note.Image, "710f8fd0-c90f-4d6a-b421-3aef173d9cf4");
                Assert.AreEqual(note.NoteCategoryId, TestHelpersNote.TestNoteCategory2().Id);
                Assert.IsTrue(note.IsActiveTask);
            }
        }

        [TestMethod]
        public async Task NotChangedWhenNullValuesAdded()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var note = await sut.EditNoteAsync(TestHelpersNote.TestNote1(),
                                                    TestHelpersNote.TestUser1().Id,
                                                   null, null, null);

                Assert.AreEqual(note.Description, TestHelpersNote.TestNote1().Description);
                Assert.AreEqual(note.Image, TestHelpersNote.TestNote1().Image);
                Assert.AreEqual(note.NoteCategoryId, TestHelpersNote.TestNoteCategory2().Id);
            }
        }
    }
}
