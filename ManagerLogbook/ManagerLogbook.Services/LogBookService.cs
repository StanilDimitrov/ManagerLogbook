using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class LogbookService : ILogbookService
    {
        private readonly ManagerLogbookContext _context;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;

        public LogbookService(ManagerLogbookContext context,
                              IBusinessUnitService businessUnitService,
                              IUserService userService)
        {
            _context = context;
            _businessUnitService = businessUnitService;
            _userService = userService;
        }

        public async Task<LogbookDTO> CreateLogbookAsync(LogbookModel model)
        {
            await CheckIfLogbookExist(model.Name);

            var logbook = new Logbook
            {
                Name = model.Name,
                Picture = model.Picture,
                BusinessUnitId = model.BusinessUnitId
            };

            _context.Logbooks.Add(logbook);
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<LogbookDTO> GetLogbookById(int logbookId)
        {
            var logbook = await GetLogbookAsync(logbookId);

            if (logbook == null)
            {
                throw new NotFoundException(ServicesConstants.LogbookNotFound);
            }

            var result = await _context.Logbooks
                                           .Include(bu => bu.BusinessUnit)
                                           .Include(n => n.Notes)
                                           .Include(n => n.UsersLogbooks)
                                               .ThenInclude( n => n.User)
                                           .FirstOrDefaultAsync(x => x.Id == logbook.Id);
            return result.ToDTO();
        }

        public async Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model)
        {
            var logbook = await GetLogbookAsync(model.Id);

            if (model.Name != logbook.Name)
            {
                await CheckIfLogbookExist(model.Name);
            }

            await SetLogbookProperties(model, logbook);
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<LogbookDTO> AddManagerToLogbookAsync(string managerId, int logbookId)
        {
            var logbook = await GetLogbookAsync(logbookId);
            var manager = await _userService.GetUserAsync(managerId);

            if (_context.UsersLogbooks.Any(u => u.UserId == managerId && u.LogbookId==logbook.Id))
            {
                throw new AlreadyExistsException(string.Format(ServicesConstants.ManagerIsAlreadyInLogbook, manager.UserName, logbook.Name));
            }

            _context.UsersLogbooks.Add(new UsersLogbooks() { UserId = manager.Id, LogbookId = logbook.Id });
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<LogbookDTO> RemoveManagerFromLogbookAsync(string managerId, int logbookId)
        {
            var logbook = await GetLogbookAsync(logbookId);
            var manager = await _userService.GetUserAsync(managerId);

            var entityToRemove = await _context.UsersLogbooks.FindAsync(manager.Id, logbookId);

            if(entityToRemove == null)
            {
                throw new NotFoundException(string.Format(ServicesConstants.ManagerIsNotPresentInLogbook, manager.UserName, logbook.Name));
            }            

            _context.UsersLogbooks.Remove(entityToRemove);
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<IReadOnlyCollection<LogbookDTO>> GetLogbooksByUserAsync(string userId)
        {
            var logbooksDTO = await _context.Logbooks
                                       .Where(ul => ul.UsersLogbooks.Any(u => u.UserId == userId))
                                       .Include(bu => bu.BusinessUnit)
                                       .Include(n => n.Notes)
                                       .Select(x => x.ToDTO())
                                       .ToListAsync();
            return logbooksDTO;
        }

        public async Task<LogbookDTO> AddLogbookToBusinessUnitAsync(int logbookId, int businessUnitId)
        {
            var logbook = await GetLogbookAsync(logbookId);
            var businessUnit = await _businessUnitService.GetBusinessUnitAsync(businessUnitId);

            logbook.BusinessUnitId = businessUnit.Id;
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        private async Task<Logbook> GetLogbookAsync(int logbookId)
        {
            var logbook = await _context.Logbooks.SingleOrDefaultAsync(lb => lb.Id == logbookId);

            if (logbook == null)
            {
                throw new NotFoundException(ServicesConstants.LogbookNotFound);
            }

            return logbook;
        }
        private async Task CheckIfLogbookExist(string logbookName)
        {
            var logbook = await _context.Logbooks.SingleOrDefaultAsync(lb => lb.Name == logbookName);

            if (logbook != null)
            {
                throw new AlreadyExistsException(string.Format(ServicesConstants.LogbookAlreadyExists, logbookName));
            }
        }

        private async Task SetLogbookProperties(LogbookModel model, Logbook entity)
        {
            entity.Name = model.Name;

            if (model.Picture != null)
            {
                entity.Picture = model.Picture;
            }

            if (model.BusinessUnitId != 0)
            {
                await _businessUnitService.GetBusinessUnitAsync(model.BusinessUnitId);
                entity.BusinessUnitId = model.BusinessUnitId;
            }
        }
    }
}
