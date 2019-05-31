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
    public class SearchTasksContainsStringAsync_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotFromLogbook));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.SearchNotesByDateAndStringStringAsync(TestHelpersNote.TestUser2().Id,
                                                                                        TestHelpersNote.TestLogbook1().Id, DateTime.MinValue, DateTime.MinValue, null));
                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes));
            }
        }

        [TestMethod]
        public async Task Return_RightCollectionWhenNoDatesSelected()
        {
            var options = TestUtils.GetOptions(nameof(Return_RightCollectionWhenNoDatesSelected));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote2());
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote3());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var notesDTO = await sut.SearchNotesByDateAndStringStringAsync(TestHelpersNote.TestUser1().Id,
                                                                     TestHelpersNote.TestLogbook1().Id, DateTime.MinValue, DateTime.MinValue, "room37");
                Assert.AreEqual(notesDTO.Count, 2);
            }
        }
    }
}

