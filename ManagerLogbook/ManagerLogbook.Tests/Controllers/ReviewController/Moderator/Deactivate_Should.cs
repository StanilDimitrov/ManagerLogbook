using ManagerLogbook.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Moderator.Controllers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Contracts.Providers;

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
                isVisible = true,
                Rating = 2
            };

            reviewServiceMock.Setup(x => x.MakeInVisibleReviewAsync(reviewViewModel.Id)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO02());

            var actionResult = await sut.Deactivate(reviewViewModel.Id);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewWasNotFound()
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
                isVisible = true,
                Rating = 2
            };

            reviewServiceMock.Setup(x => x.MakeInVisibleReviewAsync(reviewViewModel.Id)).ThrowsAsync(new NotFoundException());

            var actionResult = await sut.Deactivate(reviewViewModel.Id);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewThrowException()
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
                isVisible = true,
                Rating = 2
            };

            reviewServiceMock.Setup(x => x.MakeInVisibleReviewAsync(reviewViewModel.Id)).ThrowsAsync(new Exception());

            var actionResult = await sut.Deactivate(reviewViewModel.Id);

            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
