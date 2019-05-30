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

namespace ManagerLogbook.Tests.Services.ReviewServiceTests
{
    [TestClass]
    public class UpdateReviewAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnUpdateReview()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnUpdateReview));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.UpdateReviewDTOAsync(1,"Edit Text of Review01");

                mockBusinessValidator.Verify(x => x.IsDescriptionInRange("Edit Text of Review01"), Times.Exactly(1));

                Assert.AreEqual(review.EditedDescription, "Edit Text of Review01");
            }
        }
    }
}
