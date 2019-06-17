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
    public class GetAllReviewsInDateRangeAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetAllReviewsInDateRange()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetAllReviewsInDateRange));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review02());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review03());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.GetAllReviewsInDateRangeAsync(TestHelperReview.Review02().CreatedOn, TestHelperReview.Review04().CreatedOn);

                mockBusinessValidator.Verify(x => x.IsDateValid(TestHelperReview.Review02().CreatedOn), Times.Exactly(2));

                Assert.AreEqual(review.Count, 2);
            }
        }
    }
}
