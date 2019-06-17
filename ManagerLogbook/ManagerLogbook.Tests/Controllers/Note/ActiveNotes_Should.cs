
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
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
    public class ActiveNotes_Should
    {
        [TestMethod]
        public async Task ReturnsBadRequest_WhenLogbook_NotSelected()
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

            var userId = TestHelpersNoteController.TestUserDTO3().Id;
           
            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO3());

            var actionResult = await sut.ActiveNotes();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequest_WhenNoteService_NotAuthorizedException()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO2());
            noteServiceMock.Setup(x => x.ShowLogbookNotesWithActiveStatusAsync(userId,1)).ThrowsAsync(new NotAuthorizedException());

            var actionResult = await sut.ActiveNotes();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequest_WhenNoteService_NotFoundException()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO2());
            noteServiceMock.Setup(x => x.ShowLogbookNotesWithActiveStatusAsync(userId, 1)).ThrowsAsync(new NotFoundException());

            var actionResult = await sut.ActiveNotes();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequest_WhenUserService_NotFoundException()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new NotFoundException());
            //noteServiceMock.Setup(x => x.ShowLogbookNotesWithActiveStatusAsync(userId, 1)).ThrowsAsync(new NotFoundException());

            var actionResult = await sut.ActiveNotes();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsRedirect_ToAction()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO2());
            noteServiceMock.Setup(x => x.ShowLogbookNotesWithActiveStatusAsync(userId, 1)).ThrowsAsync(new Exception());

            var actionResult = await sut.ActiveNotes();
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
