
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Manager.Controllers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.Note
{
    [TestClass]
    public class Edit_Should
    {
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            var actionResult = await sut.Edit(model);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task NotSucced_When_DescriptionIsNotCorretllySaved()
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO3());
            var actionResult = await sut.Edit(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Returns_BadRequest_When_NotFoundException()
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ThrowsAsync(new NotFoundException());
            var actionResult = await sut.Edit(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Returns_BadRequest_When_NotAuthorizedException()
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ThrowsAsync(new NotAuthorizedException());
            var actionResult = await sut.Edit(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Returns_BadRequest_When_ArgumentException()
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ThrowsAsync(new ArgumentException());
            var actionResult = await sut.Edit(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public async Task Returns_RedirectToAction()
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


            var model = new NoteViewModel()
            {
                Id = 1,
                Description = "Room 37 is dirty",
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            noteServiceMock.Setup(x => x.GetNoteByIdAsync(model.Id)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO2());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.EditNoteAsync(model.Id, id, model.Description, null, null)).ThrowsAsync(new Exception());
            var actionResult = await sut.Edit(model);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }

    
}

