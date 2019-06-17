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

namespace ManagerLogbook.Tests.Services.ReviewServiceTests
{
    [TestClass]
    public class GetAllReviewsByBusinessUnitIdAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetAllReviewsByBusinessUnitId()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetAllReviewsByBusinessUnitId));

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

                var review = await sut.GetAllReviewsByBusinessUnitIdAsync(1);

                Assert.AreEqual(review.Count, 2);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenBusinessUnitNotExists_ReturnGetAllReviewsByBusinessUnitId()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenBusinessUnitNotExists_ReturnGetAllReviewsByBusinessUnitId));

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

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetAllReviewsByBusinessUnitIdAsync(2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNotFound));
            }
        }
    }
}
