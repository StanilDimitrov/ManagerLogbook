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

namespace ManagerLogbook.Tests.Services.NoteServiceTests
{
    [TestClass]
    public class DeactivateNoteActiveStatus_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenNoteNotExists()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenNoteNotExists));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.DeactivateNoteActiveStatus(TestHelpersNote.TestNote1().Id,
                                                                     TestHelpersNote.TestUser1().Id));
                Assert.AreEqual(ex.Message,ServicesConstants.NoteDoesNotExists);
            }
        }

        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotFromLogbook()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotFromLogbook));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote4());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => sut.DeactivateNoteActiveStatus(TestHelpersNote.TestNote1().Id,
                                                                     TestHelpersNote.TestUser1().Id));
                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserIsNotAuthorizedToEditNote, TestHelpersNote.TestUser1().UserName));
            }
        }

        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotFound));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.DeactivateNoteActiveStatus(TestHelpersNote.TestNote1().Id,
                                                     TestHelpersNote.TestUser2().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFound);
            }
        }

        [TestMethod]
        public async Task Succeed()
        {
            var options = TestUtils.GetOptions(nameof(Succeed));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote4());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {

                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                         

                var noteDTO = await sut.DeactivateNoteActiveStatus(TestHelpersNote.TestNote4().Id,
                                                                     TestHelpersNote.TestUser1().Id);

                Assert.IsFalse(noteDTO.IsActiveTask);
            }
        }
    }
}
