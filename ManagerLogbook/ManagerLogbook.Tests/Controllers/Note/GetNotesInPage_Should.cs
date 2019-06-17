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
    public class GetNotesInPage_Should
    {
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
            userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(TestHelpersNoteController.TestUserDTO2());

            noteServiceMock.Setup(x => x.GetPageCountForNotesAsync(15, 1, null)).ReturnsAsync(1);
            var notes = new List<NoteDTO>() { TestHelpersNoteController.TestNoteDTO5() };
            noteServiceMock.Setup(x => x.SearchNotesAsync(userId, 1, model.StartDate, model.EndDate, null, "restaurant", 20, 1)).ReturnsAsync(notes);
            var actionResult = await sut.GetNotesInPage(model);
            Assert.IsInstanceOfType(actionResult, typeof(PartialViewResult));
        }
    }
}
