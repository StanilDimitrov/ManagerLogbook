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
    public class CreateNoteAsync_Should
    {
        [TestMethod]
        public async Task  ThrowsExeptionWhenDescriptionIsNull()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenDescriptionIsNull));
           
            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id,
                                                                                             TestHelpersNote.TestLogbook1().Id, null, null, null));
                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.DescriptionCanNotBeNull));
            }
        }

        [TestMethod]
        public async Task SucceedCreateWhenNoCategoryAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenNoCategoryAdded));
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
                var noteDTO = await sut.CreateNoteAsync(TestHelpersNote.TestUser1().Id, TestHelpersNote.TestLogbook1().Id,
                                                       description, image, null);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                Assert.AreEqual(noteDTO.Id, 1);
                Assert.AreEqual(noteDTO.Description, description);
                Assert.IsNull(noteDTO.NoteCategoryType);
                Assert.IsFalse(noteDTO.IsActiveTask);
            }
        }

        [TestMethod]
        public async Task SucceedCreateWhenTaskCategoryIsAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenTaskCategoryIsAdded));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory2());
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
                                                     description, image, TestHelpersNote.TestNoteCategory2().Id);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                Assert.AreEqual(noteDTO.Id, 1);
                Assert.AreEqual(noteDTO.Description, description);
                Assert.AreEqual(noteDTO.NoteCategoryType, TestHelpersNote.TestNoteCategory2().Type);
                Assert.IsTrue(noteDTO.IsActiveTask);
            }
        }
    }
}
