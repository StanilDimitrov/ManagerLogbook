﻿using ManagerLogbook.Data;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.HelpersMethods;
using ManagerLogbook.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services.BusinessUnitServiceTests
{
    [TestClass]
    public class GetBusinessUnitCategoryByIdAsync_Should
    {
        [TestMethod]
        public async Task Succeed_ReturnGetBusinessUnitCategoryById()
        {
            var options = TestUtils.GetOptions(nameof(Succeed_ReturnGetBusinessUnitCategoryById));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);

                var getBusinessUnitCategory = await sut.GetBusinessUnitCategoryByIdAsync(1);

                Assert.AreEqual(getBusinessUnitCategory.Id, 1);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenBusinessUnitCategoryNotExists_GetBusinessUnitCategoryByIdAsync_Should()
        {
            var options = TestUtils.GetOptions(nameof(ThrowsExceptionWhenBusinessUnitCategoryNotExists_GetBusinessUnitCategoryByIdAsync_Should));

            using (var arrangeContext = new ManagerLogbookContext(options))
            {
                await arrangeContext.BusinessUnitCategories.AddAsync(TestHelperBusinessUnit.TestBusinessUnitCategory01());
                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new ManagerLogbookContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessValidator>();

                var sut = new BusinessUnitService(assertContext, mockBusinessValidator.Object);
                
                var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => sut.GetBusinessUnitCategoryByIdAsync(2));

                Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitCategoryNotFound));
            }
        }
    }
}

