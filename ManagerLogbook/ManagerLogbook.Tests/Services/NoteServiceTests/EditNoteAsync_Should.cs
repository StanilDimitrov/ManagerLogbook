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

namespace ManagerLogbook.Tests.Services.NoteServiceTests
{
    [TestClass]
    public class EditNoteAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotFromLogbook));
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
                                           
                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.EditNoteAsync(TestHelpersNote.TestNote1().Id,
                                                     TestHelpersNote.TestUser2().Id, null, null, null));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote));
            }
        }

        [TestMethod]
        public async Task ThrowsExeption_WhenNoteNotExists()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenNoteNotExists));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                         
                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.EditNoteAsync(TestHelpersNote.TestNote1().Id,
                                                     TestHelpersNote.TestUser2().Id, null, null, null));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.NoteDoesNotExists));
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
                                          
                var noteDTO = await sut.EditNoteAsync(TestHelpersNote.TestNote1().Id,
                                                    TestHelpersNote.TestUser1().Id,
                                                   "The room is clean", "710f8fd0-c90f-4d6a-b421-3aef173d9cf4", TestHelpersNote.TestNoteCategory1().Id);

                Assert.AreEqual(noteDTO.Description, "The room is clean");
                Assert.AreEqual(noteDTO.Image, "710f8fd0-c90f-4d6a-b421-3aef173d9cf4");
                Assert.AreEqual(noteDTO.NoteCategoryType, TestHelpersNote.TestNoteCategory1().Type);
            }
        }

        [TestMethod]
        public async Task SuccessfullyEditNoteWithCategoryFromTypeTask()
        {
            var options = TestUtils.GetOptions(nameof(SuccessfullyEditNoteWithCategoryFromTypeTask));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                         
                var noteDTO = await sut.EditNoteAsync(TestHelpersNote.TestNote1().Id,
                                                    TestHelpersNote.TestUser1().Id,
                                                   "The room is clean", "710f8fd0-c90f-4d6a-b421-3aef173d9cf4", TestHelpersNote.TestNoteCategory2().Id);

                Assert.AreEqual(noteDTO.Description, "The room is clean");
                Assert.AreEqual(noteDTO.Image, "710f8fd0-c90f-4d6a-b421-3aef173d9cf4");
                Assert.AreEqual(noteDTO.NoteCategoryType, TestHelpersNote.TestNoteCategory2().Type);
                Assert.IsTrue(noteDTO.IsActiveTask);
            }
        }

        [TestMethod]
        public async Task NotChangedWhenNullValuesAdded()
        {
            var options = TestUtils.GetOptions(nameof(NotChangedWhenNullValuesAdded));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                          
                var noteDTO = await sut.EditNoteAsync(TestHelpersNote.TestNote1().Id,
                                                    TestHelpersNote.TestUser1().Id,
                                                   null, null, null);

                Assert.AreEqual(noteDTO.Description, TestHelpersNote.TestNote1().Description);
                Assert.AreEqual(noteDTO.Image, TestHelpersNote.TestNote1().Image);
                Assert.AreEqual(noteDTO.NoteCategoryType, TestHelpersNote.TestNoteCategory2().Type);
            }
        }
    }
}
