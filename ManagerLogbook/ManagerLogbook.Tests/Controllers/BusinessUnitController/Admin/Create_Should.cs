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

namespace ManagerLogbook.Tests.Controllers.BusinessUnitController.Admin
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task Succeed()
        {
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new BusinessUnitsController(businessUnitServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var businessUnitViewModel = new BusinessUnitViewModel()
            {
                Name = "BusinessUnit01",
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };

            businessUnitServiceMock.Setup(x => x.CreateBusinnesUnitAsync(businessUnitViewModel.Name, businessUnitViewModel.Address, businessUnitViewModel.PhoneNumber, businessUnitViewModel.Email, businessUnitViewModel.Information, businessUnitViewModel.CategoryId, businessUnitViewModel.TownId, businessUnitViewModel.Picture)).ReturnsAsync(TestHelpersBusinessUnitController.TestBusinessUnitDTO01());

            var actionResult = await sut.Create(businessUnitViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ThrowsBadRequestWhenReviewViewModelIsNotValid()
        {
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new BusinessUnitsController(businessUnitServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var businessUnitViewModel = new BusinessUnitViewModel()
            {
                Name = null,
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };

            businessUnitServiceMock.Setup(x => x.CreateBusinnesUnitAsync(businessUnitViewModel.Name, businessUnitViewModel.Address, businessUnitViewModel.PhoneNumber, businessUnitViewModel.Email, businessUnitViewModel.Information, businessUnitViewModel.CategoryId, businessUnitViewModel.TownId, businessUnitViewModel.Picture)).ReturnsAsync(TestHelpersBusinessUnitController.TestBusinessUnitDTO01());

            var actionResult = await sut.Create(businessUnitViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsArgumentExceptionTheNameIsTooLong()
        {
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new BusinessUnitsController(businessUnitServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var businessUnitViewModel = new BusinessUnitViewModel()
            {
                Name = new string ('a',1000),
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };

            businessUnitServiceMock.Setup(x => x.CreateBusinnesUnitAsync(businessUnitViewModel.Name, businessUnitViewModel.Address, businessUnitViewModel.PhoneNumber, businessUnitViewModel.Email, businessUnitViewModel.Information, businessUnitViewModel.CategoryId, businessUnitViewModel.TownId, businessUnitViewModel.Picture)).ThrowsAsync(new ArgumentException());

            var actionResult = await sut.Create(businessUnitViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsAlreadyExistsException()
        {
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new BusinessUnitsController(businessUnitServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var businessUnitViewModel = new BusinessUnitViewModel()
            {
                Name = new string('a', 1000),
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };

            businessUnitServiceMock.Setup(x => x.CreateBusinnesUnitAsync(businessUnitViewModel.Name, businessUnitViewModel.Address, businessUnitViewModel.PhoneNumber, businessUnitViewModel.Email, businessUnitViewModel.Information, businessUnitViewModel.CategoryId, businessUnitViewModel.TownId, businessUnitViewModel.Picture)).ThrowsAsync(new AlreadyExistsException());

            var actionResult = await sut.Create(businessUnitViewModel);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ThrowsException()
        {
            var businessUnitServiceMock = new Mock<IBusinessUnitService>();
            var userServiceMock = new Mock<IUserService>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new BusinessUnitsController(businessUnitServiceMock.Object, userServiceMock.Object, imageOptimizerMock.Object);

            var businessUnitViewModel = new BusinessUnitViewModel()
            {
                Name = new string('a', 1000),
                Address = "Cerni Vryh",
                PhoneNumber = "1234567890",
                Email = "abv@email.com",
                Information = "this is an information",
                CategoryId = 1,
                TownId = 1,
                BusinessUnitPicture = null
            };

            businessUnitServiceMock.Setup(x => x.CreateBusinnesUnitAsync(businessUnitViewModel.Name, businessUnitViewModel.Address, businessUnitViewModel.PhoneNumber, businessUnitViewModel.Email, businessUnitViewModel.Information, businessUnitViewModel.CategoryId, businessUnitViewModel.TownId, businessUnitViewModel.Picture)).ThrowsAsync(new Exception());

            var actionResult = await sut.Create(businessUnitViewModel);

            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
