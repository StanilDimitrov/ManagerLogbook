using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.NoteServiceTests
{
    [TestClass]
    public class ShowNoteCategoriesAsync
    {
        [TestMethod]
        public async Task Return_RightCollection()
        {
            var options = TestUtils.GetOptions(nameof(Return_RightCollection));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory1());
                await arrangeContext.NoteCategories.AddAsync(TestHelpersNote.TestNoteCategory2());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedValidator = new Mock<IBusinessValidator>();
                var sut = new NoteService(assertContext, mockedValidator.Object);

                var noteCategoriesDTO = await sut.GetNoteCategoriesAsync();
                Assert.AreEqual(noteCategoriesDTO.Count, 2);
            }
        }
    }
}
