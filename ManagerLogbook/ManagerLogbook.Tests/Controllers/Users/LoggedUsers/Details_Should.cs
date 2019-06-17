using ManagerLogbook.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ManagerLogbook.Web.Controllers;
using System.Threading.Tasks;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Tests.HelpersMethods;
using Microsoft.AspNetCore.Mvc;
using ManagerLogbook.Services.CustomExeptions;

namespace ManagerLogbook.Tests.Controllers.LoggedUsers
{
    [TestClass]
    public class Details_Should
    {
        [TestMethod]
        public async Task Succeed()
        {
            var userServiceMock = new Mock<IUserService>();

            var sut = new UsersController(userServiceMock.Object);
            userServiceMock.Setup(x => x.GetUserByIdAsync(TestHelpersNoteController.TestUserDTO1().Id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            var model = new IndexUserViewModel();
            //model.User = TestHelpersUsersController.TestUserViewModel1();
            var actionResult = await sut.Details(TestHelpersNoteController.TestUserDTO1().Id);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWithNotFoundException()
        {
            var userServiceMock = new Mock<IUserService>();

            var sut = new UsersController(userServiceMock.Object);
            userServiceMock.Setup(x => x.GetUserByIdAsync(TestHelpersNoteController.TestUserDTO1().Id)).ThrowsAsync(new NotFoundException());
            var model = new IndexUserViewModel();
            //model.User = TestHelpersUsersController.TestUserViewModel1();
            var actionResult = await sut.Details(TestHelpersNoteController.TestUserDTO1().Id);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Returns_RedirectToAction()
        {
            var userServiceMock = new Mock<IUserService>();

            var sut = new UsersController(userServiceMock.Object);
            userServiceMock.Setup(x => x.GetUserByIdAsync(TestHelpersNoteController.TestUserDTO1().Id)).ThrowsAsync(new Exception());
            var model = new IndexUserViewModel();
            var actionResult = await sut.Details(TestHelpersNoteController.TestUserDTO1().Id);
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
