using ManagerLogbook.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerLogbook.Web.Areas.Manager.Controllers;
using ManagerLogbook.Web.Models;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;
using ManagerLogbook.Tests.HelpersMethods;

namespace ManagerLogbook.Tests.Controllers.NoteController
{
    [TestClass]
    public class Create_Should
    {
        //[TestMethod]
        //public async Task Succeed()
        //{
        //    var userServiceMock = new Mock<IUserService>();
        //    var noteServiceMock = new Mock<INoteService>();
        //    var logbookServiceMock = new Mock<ILogbookService>();
        //    var memoryCacheMock = new Mock<IMemoryCache>();
        //    var imageOptimizerMock = new Mock<IImageOptimizer>();

        //    var sut = new NotesController(imageOptimizerMock.Object,
        //                                   userServiceMock.Object,
        //                                   noteServiceMock.Object,
        //                                   logbookServiceMock.Object,
        //                                   memoryCacheMock.Object);


        //    var noteViewModel = new NoteViewModel()
        //    {
        //        Id = 1,
        //        Description = "Room 37 is dirty",
        //        Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
        //        CreatedOn = DateTime.Now.AddDays(-2),
        //    };
        //    var id = TestHelpersNoteController.TestUserDTO1().Id;
        //    userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
        //    var actionResult = await sut.Create(TestHelpersNoteController.TestNoteViewModel1());
        //    Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        //}
              
    }
}
