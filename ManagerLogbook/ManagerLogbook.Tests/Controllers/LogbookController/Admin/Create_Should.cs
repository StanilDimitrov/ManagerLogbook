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
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using ManagerLogbook.Web.Areas.Admin.Controllers;
using ManagerLogbook.Services.CustomExeptions;

namespace ManagerLogbook.Tests.Controllers.LogbookController.Admin
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task Succeed()
        {
            var logbookServiceMock = new Mock<ILogbookService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var logbookViewModel = new LogbookViewModel()
            {
                Id = 1,
                Name = "Logbook01",
                BusinessUnitId = 1,
                LogbookPicture = null
            };

            logbookServiceMock.Setup(x => x.CreateLogbookAsync(logbookViewModel.Name, logbookViewModel.BusinessUnitId, logbookViewModel.Picture)).ReturnsAsync(TestHelpersLogbookController.TestLogbookDTO01());

            var actionResult = await sut.Create(logbookViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewViewModelIsNotValid()
        {
            var logbookServiceMock = new Mock<ILogbookService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var logbookViewModel = new LogbookViewModel()
            {
                Id = 1,
                Name = null,
                BusinessUnitId = 1,
                LogbookPicture = null
            };

            logbookServiceMock.Setup(x => x.CreateLogbookAsync(logbookViewModel.Name, logbookViewModel.BusinessUnitId, logbookViewModel.Picture)).ReturnsAsync(TestHelpersLogbookController.TestLogbookDTO01());

            var actionResult = await sut.Create(logbookViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsArgumentExceptionTheNameIsTooLong()
        {
            var logbookServiceMock = new Mock<ILogbookService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var logbookViewModel = new LogbookViewModel()
            {
                Id = 1,
                Name = new string ('a',1000),
                BusinessUnitId = 1,
                LogbookPicture = null
            };

            logbookServiceMock.Setup(x => x.CreateLogbookAsync(logbookViewModel.Name, logbookViewModel.BusinessUnitId, logbookViewModel.Picture)).ThrowsAsync(new ArgumentException());

            var actionResult = await sut.Create(logbookViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsAlreadyExistsException()
        {
            var logbookServiceMock = new Mock<ILogbookService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var logbookViewModel = new LogbookViewModel()
            {
                Id = 1,
                Name = "Logbook01",
                BusinessUnitId = 1,
                LogbookPicture = null
            };

            logbookServiceMock.Setup(x => x.CreateLogbookAsync(logbookViewModel.Name, logbookViewModel.BusinessUnitId, logbookViewModel.Picture)).ThrowsAsync(new AlreadyExistsException());

            var actionResult = await sut.Create(logbookViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsException()
        {
            var logbookServiceMock = new Mock<ILogbookService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var logbookViewModel = new LogbookViewModel()
            {
                Id = 1,
                Name = "Logbook01",
                BusinessUnitId = 1,
                LogbookPicture = null
            };

            logbookServiceMock.Setup(x => x.CreateLogbookAsync(logbookViewModel.Name, logbookViewModel.BusinessUnitId, logbookViewModel.Picture)).ThrowsAsync(new Exception());

            var actionResult = await sut.Create(logbookViewModel);

            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
