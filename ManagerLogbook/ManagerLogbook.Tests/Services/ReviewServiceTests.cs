using AutoFixture;
using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services
{
    [TestClass]
    public class ReviewServiceTests
    {
        #region Members
        private Mock<IUserService> _mockUserService;
        private Mock<IBusinessUnitService> _mockBusinessUnitService;
        private Mock<IReviewEditor> _mockReviewEditor;
        private Mock<IBusinessValidator> _mockBusinessValidator;
        private static Fixture _fixture;
        private static DbContextOptions _options;
        private static ManagerLogbookContext _context;
        private static ReviewService _reviewService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockBusinessUnitService = new Mock<IBusinessUnitService>();
            _mockReviewEditor = new Mock<IReviewEditor>();
            _mockBusinessValidator = new Mock<IBusinessValidator>();
            _fixture = new Fixture();
            _options = TestUtils.GetOptions(_fixture.Create<string>());
            _context = new ManagerLogbookContext(_options);
            _reviewService = new ReviewService(
                _context,
                _mockBusinessValidator.Object,
                _mockBusinessUnitService.Object,
                _mockUserService.Object,
                _mockReviewEditor.Object);
        }
        #endregion

        #region CreateReviewAsync
        [TestMethod]
        public async Task CreateReviewAsync_Succeed()
        {
            var editedDescription = _fixture.Create<string>();
            var visibility = _fixture.Create<bool>();
            
            var businessUnit = _fixture.Build<BusinessUnit>()
                .Without(x => x.Reviews)
                .Without(x => x.Logbooks)
                .Without(x => x.Users)
                .Without(x => x.CensoredWords)
                .Without(x => x.Town)
                .Without(x => x.BusinessUnitCategory)
                .Create();

            var model = _fixture.Create<ReviewModel>();

            _mockReviewEditor.Setup(x => x.AutomaticReviewEditor(model.OriginalDescription)).Returns(editedDescription).Verifiable();
            _mockReviewEditor.Setup(x => x.CheckReviewVisibility(editedDescription)).Returns(visibility).Verifiable();
            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(model.BusinessUnitId)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _reviewService.CreateReviewAsync(model);
            Assert.IsInstanceOfType(result, typeof(ReviewDTO));
            Assert.AreEqual(1, _context.Reviews.Count());
            Assert.AreEqual(1, result.Id);

            _mockReviewEditor.Verify();
            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region UpdateReviewAsync
        [TestMethod]
        public async Task UpdateReviewAsync_Succeed()
        {
            var review = _fixture.Build<Review>()
                .Without(x => x.BusinessUnit)
                .Create();
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Reviews.Add(review);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<ReviewModel>()
                .With(x => x.Id, review.Id)
                .Create();
           
            var result = await _reviewService.UpdateReviewAsync(model);
            Assert.IsInstanceOfType(result, typeof(ReviewDTO));
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.EditedDescription, result.EditedDescription);
        }

        [TestMethod]
        public async Task UpdateLogbookAsync_ThrowsException_WhenReviewNotFound()
        {
            var model = _fixture.Create<ReviewModel>();
               
            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _reviewService.UpdateReviewAsync(model));
            Assert.AreEqual(ex.Message, ServicesConstants.ReviewNotFound);
        }
        #endregion

        #region MakeReviewInvisibleAsync
        [TestMethod]
        public async Task MakeReviewInvisibleAsync_Succeed()
        {
            var review = _fixture.Build<Review>()
                .Without(x => x.BusinessUnit)
                .Create();
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Reviews.Add(review);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _reviewService.MakeReviewInvisibleAsync(review.Id);
            Assert.IsInstanceOfType(result, typeof(ReviewDTO));
            Assert.IsFalse(result.isVisible);
        }

        [TestMethod]
        public async Task MakeReviewInvisibleAsync_ThrowsException_WhenReviewNotFound()
        {
            var reviewId = _fixture.Create<int>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _reviewService.MakeReviewInvisibleAsync(reviewId));
            Assert.AreEqual(ex.Message, ServicesConstants.ReviewNotFound);
        }
        #endregion

        #region GetReviewsByBusinessUnitAsync
        [TestMethod]
        public async Task GetReviewsByBusinessUnitAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
               .Without(x => x.Reviews)
               .Without(x => x.Logbooks)
               .Without(x => x.Users)
               .Without(x => x.CensoredWords)
               .Without(x => x.Town)
               .Without(x => x.BusinessUnitCategory)
               .Create();

            var reviews = new List<Review>()
            {
                _fixture.Build<Review>()
                .With(x => x.isVisible, true)
                .With(x => x.BusinessUnitId, businessUnit.Id)
                .Without(x => x.BusinessUnit)
                .Create(),
                 _fixture.Build<Review>()
                .With(x => x.isVisible, true)
                .With(x => x.BusinessUnitId, businessUnit.Id)
                .Without(x => x.BusinessUnit)
                .Create()
            };
            
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Reviews.AddRange(reviews);
                await arrangeContext.SaveChangesAsync();
            }

            _mockBusinessUnitService.Setup(x => x.GetBusinessUnitAsync(businessUnit.Id)).ReturnsAsync(businessUnit).Verifiable();

            var result = await _reviewService.GetReviewsByBusinessUnitAsync(businessUnit.Id);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<ReviewDTO>));
            Assert.AreEqual(reviews.Count, result.Count());

            _mockBusinessUnitService.Verify();
        }
        #endregion

        #region GetReviewsByModeratorAsync
        [TestMethod]
        public async Task GetReviewsByModeratorAsync_Succeed()
        {
            var moderator = _fixture.Build<User>()
               .Without(x => x.BusinessUnit)
               .Without(x => x.Notes)
               .Without(x => x.UsersLogbooks)
               .Create();

            var reviews = new List<Review>()
            {
                _fixture.Build<Review>()
                .With(x => x.isVisible, true)
                .With(x => x.BusinessUnitId, moderator.BusinessUnitId)
                .Without(x => x.BusinessUnit)
                .Create(),
                 _fixture.Build<Review>()
                .With(x => x.isVisible, true)
                .With(x => x.BusinessUnitId, moderator.BusinessUnitId)
                .Without(x => x.BusinessUnit)
                .Create()
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Reviews.AddRange(reviews);
                await arrangeContext.SaveChangesAsync();
            }

            _mockUserService.Setup(x => x.GetUserAsync(moderator.Id)).ReturnsAsync(moderator).Verifiable();

            var result = await _reviewService.GetReviewsByModeratorAsync(moderator.Id);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<ReviewDTO>));
            Assert.AreEqual(reviews.Count, result.Count());

            _mockUserService.Verify();
        }
        #endregion
    }
}
