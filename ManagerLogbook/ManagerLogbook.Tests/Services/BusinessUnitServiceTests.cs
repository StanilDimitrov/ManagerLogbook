using AutoFixture;
using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using ManagerLogbook.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Tests.Services
{
    [TestClass]
    public class BusinessUnitServiceTests
    {
        #region Members
        private Mock<IUserService> _mockUserService;
        private static Fixture _fixture;
        private static DbContextOptions _options;
        private static ManagerLogbookContext _context;
        private static BusinessUnitService _businessUnitService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _fixture = new Fixture();
            _options = TestUtils.GetOptions(_fixture.Create<string>());
            _context = new ManagerLogbookContext(_options);
            _businessUnitService = new BusinessUnitService(_context, _mockUserService.Object);
        }
        #endregion

        #region CreateBusinessUnitAsync
        [TestMethod]
        public async Task CreateBusinessUnitAsync_Succeed()
        {
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                var businessUnitCategory = _fixture.Build<BusinessUnitCategory>()
                    .Without(x => x.BusinessUnits)
                    .Create();
                var town = _fixture.Build<Town>()
                    .Without(x => x.BusinessUnits)
                    .Create();
                arrangeContext.BusinessUnitCategories.Add(businessUnitCategory);
                arrangeContext.Towns.Add(town);

                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Create<BusinessUnitModel>();
            var result = await _businessUnitService.CreateBusinnesUnitAsync(model);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Information, result.Information);
            Assert.AreEqual(model.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(model.Email, result.Email);
        }

        [TestMethod]
        public async Task CreateBusinessUnitAsync_ThrowsException_WhenNameAlreadyExists()
        {
            var brandName = _fixture.Create<string>();
            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                var businessUnit = _fixture.Build<BusinessUnit>()
                    .With(x => x.Name, brandName)
                    .Without(x => x.Reviews)
                    .Without(x => x.Logbooks)
                    .Without(x => x.Users)
                    .Without(x => x.CensoredWords)
                    .Without(x => x.Town)
                    .Without(x => x.BusinessUnitCategory)
                    .Create();

                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<BusinessUnitModel>()
                .With(x => x.Name, brandName)
                .Create();

            var ex = await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => _businessUnitService.CreateBusinnesUnitAsync(model));
            Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNameAlreadyExists, brandName));
        }
        #endregion

        #region UpdateBusinessUnitAsync
        [TestMethod]
        public async Task UpdateBusinessUnitAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .With(x => x.Id, businessUnitId)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<BusinessUnitModel>()
                .With(x => x.Id, businessUnitId)
                .Create();
            var result = await _businessUnitService.UpdateBusinessUnitAsync(model);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Information, result.Information);
            Assert.AreEqual(model.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(model.Email, result.Email);
        }

        [TestMethod]
        public async Task UpdateBusinessUnitAsync_ThrowsException_WhenBusinessUnitNotFound()
        {
            var model = _fixture.Build<BusinessUnitModel>()
                .Create();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _businessUnitService.UpdateBusinessUnitAsync(model));
            Assert.AreEqual(ex.Message, ServicesConstants.BusinessUnitNotFound);
        }

        [TestMethod]
        public async Task UpdateBusinessUnitAsync_ThrowsException_WhenNameExists()
        {
            var brandName = _fixture.Create<string>();
            var businessUnitId = _fixture.Create<int>();
            var businessUnits = new List<BusinessUnit>
                {
                    _fixture.Build<BusinessUnit>()
                   .With(x => x.Id, businessUnitId)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create(),

                _fixture.Build<BusinessUnit>()
                  .With(x => x.Name, brandName)
                  .Without(x => x.Reviews)
                  .Without(x => x.Logbooks)
                  .Without(x => x.Users)
                  .Without(x => x.CensoredWords)
                  .Without(x => x.Town)
                  .Without(x => x.BusinessUnitCategory)
                  .Create()
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.AddRange(businessUnits);
                await arrangeContext.SaveChangesAsync();
            }

            var model = _fixture.Build<BusinessUnitModel>()
                .With(x => x.Name, brandName)
                .With(x => x.Id, businessUnitId)
                .Create();

            var ex = await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => _businessUnitService.UpdateBusinessUnitAsync(model));
            Assert.AreEqual(ex.Message, string.Format(ServicesConstants.BusinessUnitNameAlreadyExists, brandName));
        }
        #endregion

        #region GetBusinessUnitDtoAsync
        [TestMethod]
        public async Task GetBusinessUnitDtoAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .With(x => x.Id, businessUnitId)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetBusinessUnitDtoAsync(businessUnitId);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));
            Assert.AreEqual(result.Id, businessUnitId);
        }

        public async Task GetBusinessUnitDtoAsync_ThrowsException_WhenBusinessUnitNotFound()
        {
            var businessUnitId = _fixture.Create<int>();
            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _businessUnitService.GetBusinessUnitDtoAsync(businessUnitId));
            Assert.AreEqual(ex.Message, ServicesConstants.BusinessUnitNotFound);
        }
        #endregion

        #region GetBusinessUnitsAsync
        [TestMethod]
        public async Task GetBusinessUnitsAsync_Succeed()
        {
            var town = _fixture.Build<Town>()
                .Without(x => x.BusinessUnits)
                .Create();
            var category = _fixture.Build<BusinessUnitCategory>()
                .Without(x => x.BusinessUnits)
                .Create();

            var businessUnits = new List<BusinessUnit>
                {
                    _fixture.Build<BusinessUnit>()
                    .With(x => x.TownId, town.Id)
                    .With(x => x.BusinessUnitCategoryId, category.Id)
                    .Without(x => x.Reviews)
                    .Without(x => x.Logbooks)
                    .Without(x => x.Users)
                    .Without(x => x.CensoredWords)
                    .Without(x => x.Town)
                    .Without(x => x.BusinessUnitCategory)
                    .Create(),
                    _fixture.Build<BusinessUnit>()
                  .With(x => x.TownId, town.Id)
                  .With(x => x.BusinessUnitCategoryId, category.Id)
                  .Without(x => x.Reviews)
                  .Without(x => x.Logbooks)
                  .Without(x => x.Users)
                  .Without(x => x.CensoredWords)
                  .Without(x => x.Town)
                  .Without(x => x.BusinessUnitCategory)
                  .Create()
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Towns.Add(town);
                arrangeContext.BusinessUnitCategories.Add(category);
                arrangeContext.BusinessUnits.AddRange(businessUnits);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetBusinessUnitsAsync();
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<BusinessUnitDTO>));
            Assert.AreEqual(result.Count, businessUnits.Count);
        }
        #endregion

        #region GetAllBusinessUnitsByCategoryIdAsync
        [TestMethod]
        public async Task GetAllBusinessUnitsByCategoryIdAsync_Succeed()
        {
            var town = _fixture.Build<Town>()
                .Without(x => x.BusinessUnits)
                .Create();
            var category = _fixture.Build<BusinessUnitCategory>()
                .Without(x => x.BusinessUnits)
                .Create();

            var businessUnits = new List<BusinessUnit>
                {
                    _fixture.Build<BusinessUnit>()
                    .With(x => x.TownId, town.Id)
                    .With(x => x.BusinessUnitCategoryId, category.Id)
                    .Without(x => x.Reviews)
                    .Without(x => x.Logbooks)
                    .Without(x => x.Users)
                    .Without(x => x.CensoredWords)
                    .Without(x => x.Town)
                    .Without(x => x.BusinessUnitCategory)
                    .Create(),
                    _fixture.Build<BusinessUnit>()
                  .With(x => x.TownId, town.Id)
                  .With(x => x.BusinessUnitCategoryId, category.Id)
                  .Without(x => x.Reviews)
                  .Without(x => x.Logbooks)
                  .Without(x => x.Users)
                  .Without(x => x.CensoredWords)
                  .Without(x => x.Town)
                  .Without(x => x.BusinessUnitCategory)
                  .Create()
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Towns.Add(town);
                arrangeContext.BusinessUnitCategories.Add(category);
                arrangeContext.BusinessUnits.AddRange(businessUnits);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetBusinessUnitsAsync();
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<BusinessUnitDTO>));
            Assert.AreEqual(result.Count, businessUnits.Count);
        }
        #endregion

        #region GetBusinessUnitsCategoriesIAsync
        [TestMethod]
        public async Task GetBusinessUnitsCategoriesIAsync_Succeed()
        {
            var town = _fixture.Build<Town>()
                .Without(x => x.BusinessUnits)
                .Create();
            var businessUnitsCategories = new List<BusinessUnitCategory>
                {
                    _fixture.Build<BusinessUnitCategory>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
                    _fixture.Build<BusinessUnitCategory>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
                    _fixture.Build<BusinessUnitCategory>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnitCategories.AddRange(businessUnitsCategories);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetBusinessUnitsCategoriesAsync();
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<BusinessUnitCategoryDTO>));
            Assert.AreEqual(result.Count, businessUnitsCategories.Count);
        }
        #endregion

        #region GetTownsAsync
        [TestMethod]
        public async Task GetTownsAsync_Succeed()
        {
            var towns = new List<Town>
                {
                    _fixture.Build<Town>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
                    _fixture.Build<Town>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
                    _fixture.Build<Town>()
                    .Without(x => x.BusinessUnits)
                    .Create(),
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Towns.AddRange(towns);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetAllTownsAsync();
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<TownDTO>));
            Assert.AreEqual(result.Count, towns.Count);
        }
        #endregion

        #region GetLogbooksForBusinessUnitAsync
        [TestMethod]
        public async Task GetLogbooksForBusinessUnitAsync_Succeed()
        {
            var town = _fixture.Build<Town>()
                .Without(x => x.BusinessUnits)
                .Create();
            var user = _fixture.Build<User>()
                .Without(x => x.BusinessUnit)
                .Without(x => x.UsersLogbooks)
                .Without(x => x.Notes)
                .Create();
            var note = _fixture.Build<Note>()
                .With(x => x.UserId, user.Id)
                .Without(x => x.NoteCategory)
                .Without(x => x.User)
                .Without(x => x.Logbook)
                .Create();

            var businessUnitId = _fixture.Create<int>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                 .With(x => x.TownId, town.Id)
                 .With(x => x.Id, businessUnitId)
                 .Without(x => x.Reviews)
                 .Without(x => x.Logbooks)
                 .Without(x => x.Users)
                 .Without(x => x.CensoredWords)
                 .Without(x => x.Town)
                 .Without(x => x.BusinessUnitCategory)
                 .Create();

            var logbooks = new List<Logbook>
                {
                    _fixture.Build<Logbook>()
                    .With(x => x.BusinessUnitId, businessUnitId)
                    .Without(x => x.BusinessUnit)
                    .Without(x => x.Notes)
                    .Without(x => x.UsersLogbooks)
                    .Create(),
                     _fixture.Build<Logbook>()
                    .With(x => x.BusinessUnitId, businessUnitId)
                    .Without(x => x.BusinessUnit)
                    .Without(x => x.Notes)
                    .Without(x => x.UsersLogbooks)
                    .Create(),
                     _fixture.Build<Logbook>()
                    .With(x => x.BusinessUnitId, businessUnitId)
                    .Without(x => x.BusinessUnit)
                    .Without(x => x.Notes)
                    .Without(x => x.UsersLogbooks)
                    .Create(),
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.Towns.Add(town);
                arrangeContext.Users.Add(user);
                arrangeContext.Notes.Add(note);
                arrangeContext.BusinessUnits.Add(businessUnit);
                arrangeContext.Logbooks.AddRange(logbooks);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetLogbooksForBusinessUnitAsync(businessUnitId);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<LogbookDTO>));
            Assert.AreEqual(result.Count, logbooks.Count);
        }
        #endregion

        #region GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync
        [TestMethod]
        public async Task GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync_Succeed()
        {
            var categories = new List<BusinessUnitCategory>()
            {
                _fixture.Build<BusinessUnitCategory>()
                .Without(x => x.BusinessUnits)
                .Create(),
                 _fixture.Build<BusinessUnitCategory>()
                .Without(x => x.BusinessUnits)
                .Create()
            };

            var businessUnits = new List<BusinessUnit>
                {
                    _fixture.Build<BusinessUnit>()
                    .With(x => x.BusinessUnitCategoryId, categories[0].Id)
                    .Without(x => x.Reviews)
                    .Without(x => x.Logbooks)
                    .Without(x => x.Users)
                    .Without(x => x.CensoredWords)
                    .Without(x => x.Town)
                    .Without(x => x.BusinessUnitCategory)
                    .Create(),
                    _fixture.Build<BusinessUnit>()
                  .With(x => x.BusinessUnitCategoryId, categories[1].Id)
                  .Without(x => x.Reviews)
                  .Without(x => x.Logbooks)
                  .Without(x => x.Users)
                  .Without(x => x.CensoredWords)
                  .Without(x => x.Town)
                  .Without(x => x.BusinessUnitCategory)
                  .Create()
            };

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnitCategories.AddRange(categories);
                arrangeContext.BusinessUnits.AddRange(businessUnits);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync();
            Assert.IsInstanceOfType(result, typeof(IReadOnlyDictionary<string, int>));
            Assert.AreEqual(result.Count, categories.Count);
            Assert.AreEqual(result[categories[0].Name], 1);
            Assert.AreEqual(result[categories[1].Name], 1);
        }
        #endregion

        #region GetBusinessUnitsCategoriesWithCountOfBusinessUnitsAsync
        [TestMethod]
        public async Task SearchBusinessUnitsAsync_Succeed()
        {
            var category = _fixture.Build<BusinessUnitCategory>()
                .Without(x => x.BusinessUnits)
                .Create();
            var town = _fixture.Build<Town>()
              .Without(x => x.BusinessUnits)
              .Create();

            var businessUnit = _fixture.Build<BusinessUnit>()
             .With(x => x.BusinessUnitCategoryId, category.Id)
             .With(x => x.TownId, town.Id)
             .Without(x => x.Reviews)
             .Without(x => x.Logbooks)
             .Without(x => x.Users)
             .Without(x => x.CensoredWords)
             .Without(x => x.Town)
             .Without(x => x.BusinessUnitCategory)
             .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnitCategories.Add(category);
                arrangeContext.Towns.Add(town);
                arrangeContext.BusinessUnits.AddRange(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.SearchBusinessUnitsAsync(businessUnit.Name, category.Id, town.Id);
            Assert.IsInstanceOfType(result, typeof(IReadOnlyCollection<BusinessUnitDTO>));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.ToList()[0].Name, businessUnit.Name);
        }
        #endregion

        #region AddLikeToBusinessUnitAsync
        [TestMethod]
        public async Task AddLikeToBusinessUnitAsync_Succeed()
        {
            var businessUnitId = _fixture.Create<int>();
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .With(x => x.Id, businessUnitId)
                   .Without(x => x.Likes)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }

            var result = await _businessUnitService.AddLikeToBusinessUnitAsync(businessUnitId);
            Assert.IsInstanceOfType(result, typeof(BusinessUnitDTO));
            Assert.AreEqual(1, result.Likes);
        }

        [TestMethod]
        public async Task AddLikeToBusinessUnitAsync_ThrowsException_WhenBusinessUnitNotFound()
        {

            var businessUnitId = _fixture.Create<int>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _businessUnitService.AddLikeToBusinessUnitAsync(businessUnitId));
            Assert.AreEqual(ex.Message, ServicesConstants.BusinessUnitNotFound);
        }
        #endregion

        #region AddModeratorToBusinessUnitsAsync
        [TestMethod]
        public async Task AddModeratorToBusinessUnitsAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Likes)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();
            var user = _fixture.Build<User>()
                  .Without(x => x.Notes)
                  .Without(x => x.BusinessUnit)
                  .Without(x => x.UsersLogbooks)
                  .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }
            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();
            var result = await _businessUnitService.AddModeratorToBusinessUnitsAsync(user.Id, businessUnit.Id);
            Assert.IsInstanceOfType(result, typeof(UserDTO));
            Assert.AreEqual(user.BusinessUnitId, result.BusinessUnitId);

            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task AddModeratorToBusinessUnitsAsync_ThrowsException_WhenBusinessUnitNotFound()
        {
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _businessUnitService.AddModeratorToBusinessUnitsAsync(moderatorId, businessUnitId));
            Assert.AreEqual(ex.Message, ServicesConstants.BusinessUnitNotFound);
        }
        #endregion

        #region RemoveModeratorFromBusinessUnitsAsync
        [TestMethod]
        public async Task RemoveModeratorFromBusinessUnitsAsync_Succeed()
        {
            var businessUnit = _fixture.Build<BusinessUnit>()
                   .Without(x => x.Likes)
                   .Without(x => x.Reviews)
                   .Without(x => x.Logbooks)
                   .Without(x => x.Users)
                   .Without(x => x.CensoredWords)
                   .Without(x => x.Town)
                   .Without(x => x.BusinessUnitCategory)
                   .Create();
            var user = _fixture.Build<User>()
                  .Without(x => x.Notes)
                  .Without(x => x.BusinessUnit)
                  .Without(x => x.UsersLogbooks)
                  .Create();

            using (var arrangeContext = new ManagerLogbookContext(_options))
            {
                arrangeContext.BusinessUnits.Add(businessUnit);
                await arrangeContext.SaveChangesAsync();
            }
            _mockUserService.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user).Verifiable();
            var result = await _businessUnitService.RemoveModeratorFromBusinessUnitsAsync(user.Id, businessUnit.Id);
            Assert.IsInstanceOfType(result, typeof(UserDTO));
            Assert.AreEqual(null, result.BusinessUnitId);

            _mockUserService.Verify();
        }

        [TestMethod]
        public async Task RemoveModeratorFromBusinessUnitsAsync_ThrowsException_WhenBusinessUnitNotFound()
        {
            var businessUnitId = _fixture.Create<int>();
            var moderatorId = _fixture.Create<string>();

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _businessUnitService.RemoveModeratorFromBusinessUnitsAsync(moderatorId, businessUnitId));
            Assert.AreEqual(ex.Message, ServicesConstants.BusinessUnitNotFound);
        }
        #endregion
    }
}
