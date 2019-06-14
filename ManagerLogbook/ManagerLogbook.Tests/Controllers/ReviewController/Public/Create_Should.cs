using ManagerLogbook.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Controllers;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;

namespace ManagerLogbook.Tests.Controllers.ReviewController.Public
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task Succeed()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 1,
                OriginalDescription = "This is first review",
                BusinessUnitId = 1,
                Rating = 1
        };
                 
            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO01());

            var actionResult = await sut.Create(reviewViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewViewModelIsNotValid()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 1,
                OriginalDescription = null,
                BusinessUnitId = 1,
                Rating = 1
            };

            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO01());

            var actionResult = await sut.Create(reviewViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewWasNotCreated()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 1,
                OriginalDescription = null,
                BusinessUnitId = 1,
                Rating = 1
            };

            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO01());

            var actionResult = await sut.Create(reviewViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewWasNotCreatedNotFoundReviewId()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 3,
                OriginalDescription = null,
                BusinessUnitId = 1,
                Rating = 1
            };

            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ReturnsAsync(TestHelpersReviewController.TestReviewDTO01());

            var actionResult = await sut.Create(reviewViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewCreateReturnsArgumentException()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 1,
                OriginalDescription = new string('a',1000),
                BusinessUnitId = 1,
                Rating = 1
            };

            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ThrowsAsync(new ArgumentException());

            var actionResult = await sut.Create(reviewViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewCreateReturnsException()
        {
            var reviewServiceMock = new Mock<IReviewService>();

            var sut = new ReviewsController(reviewServiceMock.Object);

            var reviewViewModel = new ReviewViewModel()
            {
                Id = 1,
                OriginalDescription = new string('a', 1000),
                BusinessUnitId = 1,
                Rating = 1
            };

            reviewServiceMock.Setup(x => x.CreateReviewAsync(reviewViewModel.OriginalDescription, reviewViewModel.BusinessUnitId, reviewViewModel.Rating)).ThrowsAsync(new Exception());

            var actionResult = await sut.Create(reviewViewModel);

            var result = (RedirectToActionResult)actionResult;
                       
            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        //[TestMethod]
        //public async Task Succeed()
        //{
        //    var userServiceMock = new Mock<IUserService>();
        //    var noteServiceMock = new Mock<INoteService>();
        //    var logbookServiceMock = new Mock<ILogbookService>();
        //    var memoryCacheMock = new Mock<IMemoryCache>();
        //    var imageOptimizerMock = new Mock<IImageOptimizer>();

        //    var sut = new NotesController(imageOptimizerMock.Object,
        //                                   userServiceMock.Object,
        //                                   noteServiceMock.Object,
        //                                   logbookServiceMock.Object,
        //                                   memoryCacheMock.Object);


        //    var noteViewModel = new NoteViewModel()
        //    {
        //        Id = 1,
        //        Description = "Room 37 is dirty",
        //        Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
        //        CreatedOn = DateTime.Now.AddDays(-2),
        //    };
        //    var id = TestHelpersNoteController.TestUserDTO1().Id;
        //    userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
        //    var actionResult = await sut.Create(TestHelpersNoteController.TestNoteViewModel1());
        //    Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        //}

    }
}
