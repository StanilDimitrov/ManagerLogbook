
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
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.Note
{
    [TestClass]
    public class Deactivate_Should
    {
        [TestMethod]
        public async Task ReturnsBadRequest_WhenNotSucceded()
        {
            var userServiceMock = new Mock<IUserService>();
            var rapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           rapperMock.Object);

            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(4)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.DeactivateNoteActiveStatus(TestHelpersNoteController.TestNoteDTO4().Id, id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
            var actionResult = await sut.Deactivate(4);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Succeed()
        {
            var userServiceMock = new Mock<IUserService>();
            var rapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           rapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(5)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO5());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.DeactivateNoteActiveStatus(TestHelpersNoteController.TestNoteDTO5().Id, userId)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO5());
            noteServiceMock.Setup(x => x.ShowLogbookNotesAsync(userId, 1)).ReturnsAsync(notes);
            var actionResult = await sut.Deactivate(5);
            Assert.IsInstanceOfType(actionResult, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnsBaRequest_When_NotFoundException()
        {
            var userServiceMock = new Mock<IUserService>();
            var rapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           rapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(5)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO5());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.DeactivateNoteActiveStatus(TestHelpersNoteController.TestNoteDTO5().Id, userId)).ThrowsAsync(new NotFoundException());
            noteServiceMock.Setup(x => x.ShowLogbookNotesAsync(userId, 1)).ReturnsAsync(notes);
            var actionResult = await sut.Deactivate(5);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBaRequest_When_NotAuthorizedException()
        {
            var userServiceMock = new Mock<IUserService>();
            var rapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           rapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(5)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO5());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.DeactivateNoteActiveStatus(TestHelpersNoteController.TestNoteDTO5().Id, userId)).ThrowsAsync(new NotAuthorizedException());
            noteServiceMock.Setup(x => x.ShowLogbookNotesAsync(userId, 1)).ReturnsAsync(notes);
            var actionResult = await sut.Deactivate(5);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsRedirectToAction()
        {
            var userServiceMock = new Mock<IUserService>();
            var rapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           rapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(5)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO5());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.DeactivateNoteActiveStatus(TestHelpersNoteController.TestNoteDTO5().Id, userId)).ThrowsAsync(new Exception());
            noteServiceMock.Setup(x => x.ShowLogbookNotesAsync(userId, 1)).ReturnsAsync(notes);
            var actionResult = await sut.Deactivate(5);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
