using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
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
    public class GetNoteByIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnRightNote()
        {
            var options = TestUtils.GetOptions(nameof(ReturnRightNote));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Notes.AddAsync(TestHelpersNote.TestNote1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var note = await sut.GetNoteByIdAsync(TestHelpersNote.TestNote1().Id);

                Assert.AreEqual(note.Id, TestHelpersNote.TestNote1().Id);
            }
        }

        [TestMethod]
        public async Task ReturnNull()
        {
            var options = TestUtils.GetOptions(nameof(ReturnNull));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var note = await sut.GetNoteByIdAsync(TestHelpersNote.TestNote1().Id);

                Assert.IsNull(note);
            }
        }
    }
}
