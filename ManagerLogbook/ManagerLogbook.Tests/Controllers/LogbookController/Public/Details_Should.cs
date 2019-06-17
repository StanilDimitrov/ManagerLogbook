//using ManagerLogbook.Services.Contracts;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ManagerLogbook.Web.Models;
//using ManagerLogbook.Web.Services.Contracts;
//using Microsoft.Extensions.Caching.Memory;
//using ManagerLogbook.Tests.HelpersMethods;
//using ManagerLogbook.Web.Controllers;
//using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
//using ManagerLogbook.Services.DTOs;

//namespace ManagerLogbook.Tests.Controllers.ReviewController.Public
//{
//    [TestClass]
//    public class Details_Should
//    {
//        [TestMethod]
//        public async Task Succeed()
//        {
//            var logbookServiceMock = new Mock<ILogbookService>();
//            var userServiceMock = new Mock<IUserService>();
//            var noteServiceMock = new Mock<INoteService>();



//            var sut = new LogbooksController(logbookServiceMock.Object, userServiceMock.Object, noteServiceMock.Object);

//            var indexLogbookViewModel = new IndexLogbookViewModel()
//            {
//                AssignedManagers = new List<UserDTO>(),
//                ActiveNotes = new List<NoteDTO>(),
//                TotalNotes = new List<NoteDTO>(),

//            };

//            logbookServiceMock.Setup(x => x.GetLogbookById(1))).ReturnsAsync(TestHelpersLogbookController.TestLogbookDTO01());

//            var actionResult = await sut.Create(reviewViewModel);

//            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
//        }

//        //[TestMethod]
//        //public async Task Succeed()
//        //{
//        //    var userServiceMock = new Mock<IUserService>();
//        //    var noteServiceMock = new Mock<INoteService>();
//        //    var logbookServiceMock = new Mock<ILogbookService>();
//        //    var memoryCacheMock = new Mock<IMemoryCache>();
//        //    var imageOptimizerMock = new Mock<IImageOptimizer>();

//        //    var sut = new NotesController(imageOptimizerMock.Object,
//        //                                   userServiceMock.Object,
//        //                                   noteServiceMock.Object,
//        //                                   logbookServiceMock.Object,
//        //                                   memoryCacheMock.Object);


//        //    var noteViewModel = new NoteViewModel()
//        //    {
//        //        Id = 1,
//        //        Description = "Room 37 is dirty",
//        //        Image = "abd22cec-9df6-43ea-b5aa-991689af55d1",
//        //        CreatedOn = DateTime.Now.AddDays(-2),
//        //    };
//        //    var id = TestHelpersNoteController.TestUserDTO1().Id;
//        //    userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
//        //    var actionResult = await sut.Create(TestHelpersNoteController.TestNoteViewModel1());
//        //    Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
//        //}

//    }
//}
