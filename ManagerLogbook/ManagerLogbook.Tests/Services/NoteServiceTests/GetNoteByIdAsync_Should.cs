using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
                                         
                var noteDTO = await sut.GetNoteDtoAsync(TestHelpersNote.TestNote1().Id);

                Assert.AreEqual(noteDTO.Id, TestHelpersNote.TestNoteDTO1().Id);
            }
        }

        [TestMethod]
        public async Task ThrowsException_NoteNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsException_NoteNotFound));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetNoteDtoAsync(TestHelpersNote.TestNote1().Id));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.NotNotFound));
            }
        }
    }
}
