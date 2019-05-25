using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateNoteAsync(null, null, null));
                                                     
                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.DescriptionCanNotBeNull));
            }
        }

        [TestMethod]
        public async Task SucceedCreateWhenNoCategoryAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenNoCategoryAdded));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";
                var note = await sut.CreateNoteAsync(description, image, null);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                Assert.AreEqual(note.Id, 1);
                Assert.AreEqual(note.Description, description);
                Assert.IsNull(note.NoteCategoryId);
                Assert.IsFalse(note.IsActiveTask);
            }
        }

        [TestMethod]
        public async Task SucceedCreateWhenTaskCategoryIsAdded()
        {
            var options = TestUtils.GetOptions(nameof(SucceedCreateWhenTaskCategoryIsAdded));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory2());
               
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);
                var description = "Room 37 needs cleaning.";
                var image = "abd22cec-9df6-43ea-b5aa-991689af55d1";
                var note = await sut.CreateNoteAsync(description, image, TestHelpersNote.TestNoteCategory2().Id);

                mockedValidator.Verify(x => x.IsDescriptionInRange(description), Times.Exactly(1));
                Assert.AreEqual(note.Id, 1);
                Assert.AreEqual(note.Description, description);
                Assert.AreEqual(note.NoteCategoryId, TestHelpersNote.TestNoteCategory2().Id);
                Assert.IsTrue(note.IsActiveTask);
            }
        }

    }
}
