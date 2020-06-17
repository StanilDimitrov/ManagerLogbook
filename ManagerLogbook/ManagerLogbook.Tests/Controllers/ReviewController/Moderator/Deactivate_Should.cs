using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Moderator.Controllers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.ReviewController.Moderator
{
    [TestClass]
    public class Deactivate_Should
    {
        [TestMethod]
        public async Task Succeed()
        {
            var reviewServiceMock = new Mock<IReviewService>();
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();

            var sut = new ReviewsController(reviewServiceMock.Object, businessUnitServiceMock.Object, userServiceMock.Object, wrapperMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 2,
                OriginalDescription = "This is second review",
                EditedDescription = "This is EDIT second review",
                BusinessUnitId = 1,
                IsVisible = true,
                Rating = 2
            };

            reviewServiceMock.Setup(x => x.MakeReviewInvisibleAsync(reviewViewModel.Id)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO02());

            var actionResult = await sut.Deactivate(reviewViewModel.Id);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }
    }
}
