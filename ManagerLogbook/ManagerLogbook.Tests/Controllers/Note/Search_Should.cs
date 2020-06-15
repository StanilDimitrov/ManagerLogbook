
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Web.Areas.Manager.Controllers;
using ManagerLogbook.Web.Areas.Manager.Models;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Controllers.Note
{
    [TestClass]
    public class Search_Should
    {
        [TestMethod]
        public async Task Returns_BadRequest_When_UserNotSelectedLogbook()
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
            
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5(), TestHelpersNoteController.TestNoteDTO4() };
            var categories = new List<NoteGategoryDTO>() { TestHelpersNoteController.TestNoteCategoryDTO1() };

            var model = new SearchViewModel();
           
            rapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO3());

            //logbookServiceMock.Setup(x => x.GetLogbookById(1)).ReturnsAsync(TestHelpersNoteController.TestLogbookDTO1());
            //logbookServiceMock.Setup(x => x.GetAllLogbooksByUserAsync(userId)).ReturnsAsync(logbooks);
            //noteServiceMock.Setup(x => x.Get15NotesByIdAsync(1, 1)).ReturnsAsync(notes);
            //noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1)).ReturnsAsync(1);
            //noteServiceMock.Setup(x => x.GetNoteCategoriesAsync()).ReturnsAsync(categories);
            var actionResult = await sut.Search(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Succeed_Returns_PartialView()
        {
            var userServiceMock = new Mock<IUserService>();
            var wrapperMock = new Mock<IUserServiceWrapper>();
            var noteServiceMock = new Mock<INoteService>();
            var logbookServiceMock = new Mock<ILogbookService>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var imageOptimizerMock = new Mock<IImageOptimizer>();

            var sut = new NotesController(imageOptimizerMock.Object,
                                           userServiceMock.Object,
                                           noteServiceMock.Object,
                                           logbookServiceMock.Object,
                                           memoryCacheMock.Object,
                                           wrapperMock.Object);

            var userId = TestHelpersNoteController.TestUserDTO2().Id;

            var model = new SearchViewModel() { StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue };

            wrapperMock.Setup(x => x.GetLoggedUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            userServiceMock.Setup(x => x.GetUserDtoByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO2());

            noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1,null)).ReturnsAsync(1);
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5() };
            noteServiceMock.Setup(x => x.SearchNotesAsync(userId, 1, model.StartDate, model.EndDate, null, "restaurant", 20, 1)).ReturnsAsync(notes);
            var actionResult = await sut.Search(model);
            Assert.IsInstanceOfType(actionResult, typeof(PartialViewResult));
        }
    }
}
