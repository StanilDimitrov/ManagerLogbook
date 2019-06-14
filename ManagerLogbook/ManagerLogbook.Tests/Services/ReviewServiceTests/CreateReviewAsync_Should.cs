using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.ReviewServiceTests
{
    [TestClass]
    public class CreateReviewAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnCreateReview()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnCreateReview));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.CreateReviewAsync(TestHelperReview.Review01().OriginalDescription, TestHelperReview.Review01().Rating, TestHelperReview.Review01().BusinessUnitId );

                mockBusinessValidator.Verify(x => x.IsDescriptionInRange("Original Text of Review01"), Times.Exactly(1));
                mockBusinessValidator.Verify(x => x.IsRatingInRange(1), Times.Exactly(1));
                
                Assert.AreEqual(review.OriginalDescription, "Original Text of Review01");
                Assert.AreEqual(review.Rating, 1);
            }
        }
    }
}
