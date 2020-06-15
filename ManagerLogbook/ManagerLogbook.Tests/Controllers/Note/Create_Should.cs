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

namespace ManagerLogbook.Tests.Controllers.NoteController
{
    [TestClass]
    public class Create_Should
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
                Description = "Room 37 is dirty",
                Image = null,
                
            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;
            //var id = TestHelpersNoteController.TestUserDTO1().Id;
            

            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.CreateNoteAsync(id,1, "Room 37 is dirty", null, null)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO1());
            var actionResult = await sut.Create(model);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Return_BadRequest_When_ModelIsNotValid()
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
                //Description = null,
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;
            
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.CreateNoteAsync(id, 1, null, null, null)).ThrowsAsync(new ArgumentException());
            var actionResult = await sut.Create(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Return_BadRequest_When_NotAutorizedException()
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
                //Description = null,
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.CreateNoteAsync(id, 1, null, null, null)).ThrowsAsync(new NotAuthorizedException());
            var actionResult = await sut.Create(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Return_BadRequest_When_NotFoundException()
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
                //Description = null,
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.CreateNoteAsync(id, 1, null, null, null)).ThrowsAsync(new NotFoundException());
            var actionResult = await sut.Create(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Return_RedirectToAction()
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
                //Description = null,
                Image = null,

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            noteServiceMock.Setup(x => x.CreateNoteAsync(id, 1, null, null, null)).ThrowsAsync(new Exception());
            var actionResult = await sut.Create(model);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        }
    }
}
