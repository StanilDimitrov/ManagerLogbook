using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Controllers;
using ManagerLogbook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(TestHelpersNoteController.TestUserDTO1().Id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
            var model = new IndexUserViewModel();
            //model.User = TestHelpersUsersController.TestUserViewModel1();
            var actionResult = await sut.Details(TestHelpersNoteController.TestUserDTO1().Id);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));
        }
    }
}
