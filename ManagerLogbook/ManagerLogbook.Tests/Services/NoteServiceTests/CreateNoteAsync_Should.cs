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
    public class CreateNoteAsync_Should
    {

        [TestMethod]
        public async Task SucceedCreateWhenNoCategoryAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenNoCategoryAdded));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";
                var noteDTO = await sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, null);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                mockedValidator.Verify(x => x.IsDescriptionIsNullOrEmpty(description), Times.Exactly(1));
                Assert.AreEqual(noteDTO.Id, 1);
                Assert.AreEqual(noteDTO.Description, description);
                Assert.IsNull(noteDTO.CategoryName);
                Assert.IsFalse(noteDTO.IsActiveTask);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenUserNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenUserNotFound));
            
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, TestHelpersNote.TestNoteCategory2().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFound);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenLogbookNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenUserNotFound));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, TestHelpersNote.TestNoteCategory2().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.LogbookNotFound);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenUserIsNotAuthorized()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenUserIsNotAuthorized));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";

                var ex = await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, TestHelpersNote.TestNoteCategory2().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotManagerOfLogbook, TestHelpersNote.TestUser1().UserName, TestHelpersNote.TestLogbook1().Name));
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenAddNotExistingCategory()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenAddNotExistingCategory));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, TestHelpersNote.TestNoteCategory2().Id));

                Assert.AreEqual(ex.Message,ServicesConstants.NoteCategoryDoesNotExists);

            }
        }

        [TestMethod]
        public async Task SucceedCreateWhenNoteCategoryIsAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenNoteCategoryIsAdded));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory2());
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.Logbooks.AddAsync(TestHelpersNote.TestLogbook1());
                await arrangeContext.UsersLogbooks.AddAsync(TestHelpersNote.TestUsersLogbooks1());
                await arrangeContext.SaveChangesAsync();
            }
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";
                var noteDTO = await sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                     description, image, TestHelpersNote.TestNoteCategory2().Id);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                Assert.AreEqual(noteDTO.Id, 1);
                Assert.AreEqual(noteDTO.Description, description);
                Assert.AreEqual(noteDTO.CategoryName, TestHelpersNote.TestNoteCategory2().Name);
                Assert.IsTrue(noteDTO.IsActiveTask);
            }
        }
    }
}
