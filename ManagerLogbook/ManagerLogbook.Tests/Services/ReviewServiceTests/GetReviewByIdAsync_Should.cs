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

namespace ManagerLogbook.Tests.Services.ReviewServiceTests
{
    [TestClass]
    public class GetReviewByIdAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetReviewByIdAsync_Should()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetReviewByIdAsync_Should));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review02());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.GetReviewByIdAsync(1);

                Assert.AreEqual(review.EditedDescription, TestHelperReview.Review01().EditedDescription);
                Assert.AreEqual(review.OriginalDescription, TestHelperReview.Review01().OriginalDescription);
                Assert.AreEqual(review.BusinessUnitId, TestHelperReview.Review01().BusinessUnitId);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenReviewNotExists_GetReviewByIdAsync()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenReviewNotExists_GetReviewByIdAsync));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetReviewByIdAsync(2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.ReviewNotFound));
            }
        }
    }
}
