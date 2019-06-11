using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.UserServiceTest
{
    [TestClass]
    public class GetAllModeratorsNotPresentInBusinessUnitAsync_Should
    {
        [TestMethod]
        public async Task ReturnRightCollection()
        {
            var options = TestUtils.GetOptions(nameof(ReturnRightCollection));

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceRapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);
                var moderators = new List<User>() { TestHelpersNote.TestUser1(), TestHelpersNote.TestUser2() };

                mockedRapper.Setup(x => x.GetAllUsersInRoleAsync("Moderator")).ReturnsAsync(moderators);

                var usersDTO = await sut.GetAllModeratorsNotPresentInBusinessUnitAsync(1);

                Assert.AreEqual(usersDTO.Count, 1);
            }
        }
    }
}
