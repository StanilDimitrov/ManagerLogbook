using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
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
    public class GetPageCountAForNotesAsync_Should
    {
        [TestMethod]
        public async Task ReturnRightPagesCount()
        {
            var options = TestUtils.GetOptions(nameof(ReturnRightPagesCount));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                var notes = new List<Note>();
                for (int i = 0; i < 35; i++)
                {
                    notes.Add(new Note()
                    {

                        Description = "Very nice morning.",
                        LogbookId = TestHelpersNote.TestLogbook1().Id
                    });
                }
                await arrangeContext.Notes.AddRangeAsync(notes);
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var notesPerPage = 15;

                var sut = new NoteService(assertContext, mockedValidator.Object);

                var pages = await sut.GetPageCountForNotesAsync(notesPerPage, TestHelpersNote.TestLogbook1().Id);

                Assert.AreEqual(pages,3);
            }
        }
    }
}
