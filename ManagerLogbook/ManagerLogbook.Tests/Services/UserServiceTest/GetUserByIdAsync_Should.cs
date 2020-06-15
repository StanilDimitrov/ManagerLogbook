﻿using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
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
    public class GetUserByIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnRightUser()
        {
            var options = TestUtils.GetOptions(nameof(ReturnRightUser));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.Users.AddAsync(TestHelpersNote.TestUser1());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceWrapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);

                var userDTO = await sut.GetUserDtoByIdAsync(TestHelpersNote.TestUser1().Id);

                Assert.AreEqual(userDTO.Id, TestHelpersNote.TestUser1().Id);
            }
        }

        [TestMethod]
        public async Task ThrowsExeptionWhenUserNotFound()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExeptionWhenUserNotFound));
            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockedRapper = new Mock<IUserServiceWrapper>();
                var sut = new UserService(assertContext, mockedRapper.Object);

                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetUserDtoByIdAsync(TestHelpersNote.TestUser1().Id));

                Assert.AreEqual(ex.Message, ServicesConstants.UserNotFound);
            }
        }

    }
}
