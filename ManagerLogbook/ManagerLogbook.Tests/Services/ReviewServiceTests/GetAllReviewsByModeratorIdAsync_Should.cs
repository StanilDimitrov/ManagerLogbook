﻿using ManagerLogbook.Data;
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
    public class GetAllReviewsByModeratorIdAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetAllReviewsByModeratorId()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetAllReviewsByModeratorId));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review02());
                await arrangeContext.Users.AddAsync(TestHelperReview.TestUser01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.GetAllReviewsByModeratorIdAsync(TestHelperReview.TestUser01().Id);

                Assert.AreEqual(review.Count, 2);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenModeratorNotExistsBy_ReturnGetAllReviewsByModeratorId()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenModeratorNotExistsBy_ReturnGetAllReviewsByModeratorId));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnits.AddAsync(TestHelperReview.TestBusinessUnit01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review01());
                await arrangeContext.Reviews.AddAsync(TestHelperReview.Review02());
                await arrangeContext.Users.AddAsync(TestHelperReview.TestUser01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();
                var mockReviewEditor = new Mock<IReviewEditor>();

                var sut = new ReviewService(assertContext, mockBusinessValidator.Object, mockReviewEditor.Object);

                var review = await sut.GetAllReviewsByModeratorIdAsync(TestHelperReview.TestUser01().Id);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetAllReviewsByModeratorIdAsync("2"));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.UserNotFound));
            }
        }
    }
}
