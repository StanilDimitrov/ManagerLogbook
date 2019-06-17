
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Manager.Controllers;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.Note
{
    [TestClass]
    public class GetAllLogbooksByUser_Should
    {
        [TestMethod]
        public async Task Returns_RedirectJson()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           wrapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;
            var logbooksDTO = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1() };

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooksDTO);
            
            var actionResult = await sut.GetAllLogbooksByUser();
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult));
        }

        [TestMethod]
        public async Task Returns_BadRequestWhen_NotFoundException()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           wrapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;
            var logbooksDTO = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1() };

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ThrowsAsync(new NotFoundException());

            var actionResult = await sut.GetAllLogbooksByUser();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Returns_RedirectToIndex()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           wrapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;
            var logbooksDTO = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1() };

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ThrowsAsync(new Exception());

            var actionResult = await sut.GetAllLogbooksByUser();
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        }
    }
}
