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
    public class Index_Should
    {
        //[TestMethod]
        //public async Task Succed()
        //{
        //    var userServiceMock = new Mock<IUserService>();
        //    var rapperMock = new Mock<IUserServiceWrapper>();
        //    var noteServiceMock = new Mock<INoteService>();
        //    var logbookServiceMock = new Mock<ILogbookService>();
        //    var memoryCacheMock = new Mock<IMemoryCache>();
        //    var imageOptimizerMock = new Mock<IImageOptimizer>();

        //    var sut = new NotesController(imageOptimizerMock.Object,
        //                                   userServiceMock.Object,
        //                                   noteServiceMock.Object,
        //                                   logbookServiceMock.Object,
        //                                   memoryCacheMock.Object,
        //                                   rapperMock.Object);

        //    var userId = TestHelpersNoteController.TestUserDTO1().Id;
        //    var logbooks = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1(), TestHelpersNoteController.TestLogbookDTO2() };
        //    var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };
        //    var categories = new List<NoteGategoryDTO>() { TestHelpersNoteController.TestNoteCategoryDTO1() };


        //    noteServiceMock.Setup(x => x.GetNoteByIdAsync(4)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
        //    rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
        //    userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO1());
        //    logbookServiceMock.Setup(x => x.GetLogbookById(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
        //    logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooks);
        //    noteServiceMock.Setup(x => x.Get15NotesByIdAsync(1, 1)).ReturnsAsync(notes);
        //    noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1)).ReturnsAsync(1);
        //    noteServiceMock.Setup(x => x.GetNoteCategoriesAsync()).ReturnsAsync(categories);
        //    var actionResult = await sut.Index();
        //    Assert.IsInstanceOfType(actionResult, typeof(PartialViewResult));
        //}

        [TestMethod]
        public async Task ReturnsView_When_NoLogBookSelected()
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

            var userId = TestHelpersNoteController.TestUserDTO3().Id;
            var logbooks = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1(), TestHelpersNoteController.TestLogbookDTO2() };
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };
            var categories = new List<NoteGategoryDTO>() { TestHelpersNoteController.TestNoteCategoryDTO1() };


            noteServiceMock.Setup(x => x.GetNoteByIdAsync(4)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO3());
            logbookServiceMock.Setup(x => x.GetLogbookById(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooks);
            noteServiceMock.Setup(x => x.Get15NotesByIdAsync(1, 1)).ReturnsAsync(notes);
            noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1)).ReturnsAsync(1);
            noteServiceMock.Setup(x => x.GetNoteCategoriesAsync()).ReturnsAsync(categories);
            var actionResult = await sut.Index();
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequest_When_NotFoundException()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;
            var logbooks = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1(), TestHelpersNoteController.TestLogbookDTO2() };
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };
            var categories = new List<NoteGategoryDTO>() { TestHelpersNoteController.TestNoteCategoryDTO1() };


            noteServiceMock.Setup(x => x.GetNoteByIdAsync(4)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ThrowsAsync(new NotFoundException());
            logbookServiceMock.Setup(x => x.GetLogbookById(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooks);
            noteServiceMock.Setup(x => x.Get15NotesByIdAsync(1, 1)).ReturnsAsync(notes);
            noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1)).ReturnsAsync(1);
            noteServiceMock.Setup(x => x.GetNoteCategoriesAsync()).ReturnsAsync(categories);
            var actionResult = await sut.Index();
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ReturnsBadRequest_When_Exception()
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

            var userId = TestHelpersNoteController.TestUserDTO2().Id;
            var logbooks = new List<LogbookDTO>() { TestHelpersNoteController.TestLogbookDTO1(), TestHelpersNoteController.TestLogbookDTO2() };
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };
            var categories = new List<NoteGategoryDTO>() { TestHelpersNoteController.TestNoteCategoryDTO1() };


            noteServiceMock.Setup(x => x.GetNoteByIdAsync(4)).ReturnsAsync(TestHelpersNoteController.TestNoteDTO4());
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ThrowsAsync(new Exception());
            logbookServiceMock.Setup(x => x.GetLogbookById(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooks);
            noteServiceMock.Setup(x => x.Get15NotesByIdAsync(1, 1)).ReturnsAsync(notes);
            noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1)).ReturnsAsync(1);
            noteServiceMock.Setup(x => x.GetNoteCategoriesAsync()).ReturnsAsync(categories);
            var actionResult = await sut.Index();
            var result = (RedirectToActionResult)actionResult;

            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}

