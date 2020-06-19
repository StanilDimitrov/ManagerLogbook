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
        private readonly IUserService _userService;

        public LogbookService(ManagerLogbookContext context,
                              IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<LogbookDTO> CreateLogbookAsync(LogbookModel model)
        {
            var logbook = new Logbook
            {
                Name = model.Name,
                Picture = model.Picture,
                BusinessUnitId = model.BusinessUnitId.Value
            };

            _context.Logbooks.Add(logbook);
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<LogbookDTO> GetLogbookDetailsAsync(int logbookId)
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
                               .ThenInclude(n => n.User)
                               .FirstOrDefaultAsync(x => x.Id == logbook.Id);
            return result.ToDTO();
        }

        public async Task<LogbookDTO> UpdateLogbookAsync(LogbookModel model, Logbook logbook)
        {
            SetLogbookProperties(model, logbook);
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<UserDTO> AddManagerToLogbookAsync(User manager, int logbookId)
        {
            _context.UsersLogbooks.Add(new UsersLogbooks() 
            {
                UserId = manager.Id,
                LogbookId = logbookId
            });

            await _context.SaveChangesAsync();

            return manager.ToDTO();
        }

        public async Task<UserDTO> RemoveManagerFromLogbookAsync(string managerId, int logbookId)
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

            return manager.ToDTO();
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

        public async Task<LogbookDTO> AddLogbookToBusinessUnitAsync(Logbook logbook, int businessUnitId)
        {
            logbook.BusinessUnitId = businessUnitId;
            await _context.SaveChangesAsync();

            return logbook.ToDTO();
        }

        public async Task<Logbook> GetLogbookAsync(int logbookId)
        {
            var logbook = await _context.Logbooks.SingleOrDefaultAsync(lb => lb.Id == logbookId);

            if (logbook == null)
            {
                throw new NotFoundException(ServicesConstants.LogbookNotFound);
            }

            return logbook;
        }

        public async Task<bool> CheckIfLogbookNameExist(string logbookName)
        {
            var hasNameExists = await _context.Logbooks.AnyAsync(lb => lb.Name == logbookName);

            if (hasNameExists)
            {
                throw new AlreadyExistsException(string.Format(ServicesConstants.LogbookAlreadyExists, logbookName));
            }

            return hasNameExists;
        }

        private void SetLogbookProperties(LogbookModel model, Logbook entity)
        {
            entity.Name = model.Name;

            if (model.Picture != null)
            {
                entity.Picture = model.Picture;
            }

            if (model.BusinessUnitId.HasValue)
            {
                entity.BusinessUnitId = model.BusinessUnitId.Value;
            }
        }
    }
}
