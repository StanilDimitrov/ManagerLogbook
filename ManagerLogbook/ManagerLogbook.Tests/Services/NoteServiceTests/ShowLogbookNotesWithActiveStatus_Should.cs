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
    public class ShowLogbookNotesWithActiveStatus_Should
    {
        [TestMethod]
        public async Task ThrowsExeption_WhenUserIsNotAuthorized()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeption_WhenUserIsNotAuthorized));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser3());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                   
                
                var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => sut.ShowLogbookNotesWithActiveStatusAsync(TestHelpersNote.TestUser3().Id,
                                                                               TestHelpersNote.TestLogbook1().Id));
                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserIsNotAuthorizedToViewNotes, TestHelpersNote.TestUser3().UserName));
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


                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.ShowLogbookNotesWithActiveStatusAsync(TestHelpersNote.TestUser3().Id,
                                                                               TestHelpersNote.TestLogbook1().Id));
                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFound);
            }
        }

        [TestMethod]
        public async Task Return_RightCollection()
        {
            var options = TestUtils.GetOptions(nameof(Return_RightCollection));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote2());
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote4());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                                        
                var notesDTO = await sut.ShowLogbookNotesWithActiveStatusAsync(TestHelpersNote.TestUser1().Id,
                                                                               TestHelpersNote.TestLogbook1().Id);
                Assert.AreEqual(notesDTO.Count, 1);
            }
        }
    }
}
