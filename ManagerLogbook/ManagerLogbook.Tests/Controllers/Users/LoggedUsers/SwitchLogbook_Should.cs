using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Manager.Controllers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.Users.LoggedUsers
{
    [TestClass]
    public class SwitchLogbook_Should
    {
        [TestMethod]
        public async Task Returns_BadRequest_When_LogbookIdIsEqualToNewId()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 1

            };
            var id = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            var actionResult = await sut.SwitchLogbook(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Succeed()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 2

            };
            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            userServiceMock.Setup(x => x.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value)).ReturnsAsync(TestHelpersUsersController.TestUserDTO4());
            var actionResult = await sut.SwitchLogbook(model);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Notes", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequst_NotFoundException()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 2

            };
            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ThrowsAsync(new NotFoundException());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            userServiceMock.Setup(x => x.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value)).ReturnsAsync(TestHelpersUsersController.TestUserDTO4());
            var actionResult = await sut.SwitchLogbook(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequst_NotAuthorizedException()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 3

            };
            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersUsersController.TestUserDTO4());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            userServiceMock.Setup(x => x.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value)).ThrowsAsync(new NotAuthorizedException());
            var actionResult = await sut.SwitchLogbook(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequst_NotFoundFromUserServiceException()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 3

            };
            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersUsersController.TestUserDTO4());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            userServiceMock.Setup(x => x.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value)).ThrowsAsync(new NotFoundException());
            var actionResult = await sut.SwitchLogbook(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequst_Exception()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var logbookServiceMock = new Mock<ILogbookService>();


            var sut = new UsersController(userServiceMock.Object,
                                           logbookServiceMock.Object,
                                           wrapperMock.Object);


            var model = new IndexNoteViewModel()
            {
                CurrentLogbookId = 2

            };
            var userId = TestHelpersNoteController.TestUserDTO1().Id;

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersUsersController.TestUserDTO4());
            logbookServiceMock.Setup(x => x.GetLogbookDetailsAsync(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            userServiceMock.Setup(x => x.SwitchLogbookAsync(userId, model.CurrentLogbookId.Value)).ThrowsAsync(new Exception());
            var actionResult = await sut.SwitchLogbook(model);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
