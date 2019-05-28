using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class LogbookService : ILogbookService
    {
        private readonly ManagerLogbookContext context;
        private readonly IBusinessValidator businessValidator;

        public LogbookService(ManagerLogbookContext context,
                              IBusinessValidator businessValidator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.businessValidator = businessValidator ?? throw new ArgumentNullException(nameof(businessValidator));
        }

        public async Task<LogbookDTO> CreateLogbookAsync(string name, int businessUnitId, string picture)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(ServicesConstants.NameCanNotBeNullOrEmpty);
            }

            businessValidator.IsNameInRange(name);

            var logbook = new Logbook() { Name = name, Picture = picture, BusinessUnitId = businessUnitId };

            this.context.Logbooks.Add(logbook);

            await this.context.SaveChangesAsync();

            var result = await this.context.Logbooks
                                           .Include(bu => bu.BusinessUnit)
                                           .Include(n => n.Notes)
                                           .FirstOrDefaultAsync(x => x.Id == logbook.Id);
            return result.ToDTO();
        }

        public async Task<Logbook> IsLogbookExists(int id)
        {
            return await this.context.Logbooks.SingleOrDefaultAsync(l => l.Id == id);

        }

        public async Task<LogbookDTO> UpdateLogbookAsync(int logbookId, string name, string picture)
        {
            var logbook = await this.context.Logbooks.FindAsync(logbookId);

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(ServicesConstants.NameCanNotBeNullOrEmpty);
            }

            if (name != null)
            {
                businessValidator.IsNameInRange(name);
            }

            logbook.Name = name;

            if (picture != null)
            {
                logbook.Picture = picture;
            }

            logbook.Picture = picture;

            await this.context.SaveChangesAsync();

            var result = await this.context.Logbooks
                               .Include(bu => bu.BusinessUnit)
                               .Include(n => n.Notes)
                               .FirstOrDefaultAsync(x => x.Id == logbook.Id);

            return result.ToDTO();
        }

        public async Task<LogbookDTO> AddManagerToLogbookAsync(string managerId, int logbookId)
        {
            var logbook = await this.context.Logbooks.FindAsync(logbookId);
            var manager = await this.context.Users.FindAsync(managerId);

            if (this.context.UsersLogbooks.Any(u => u.UserId == managerId))
            {
                throw new ArgumentException(string.Format(ServicesConstants.ManagerIsAlreadyInLogbook, manager.UserName, logbook.Name));
            }

            await this.context.UsersLogbooks.AddAsync(new UsersLogbooks() { UserId = manager.Id, LogbookId = logbook.Id });

            await this.context.SaveChangesAsync();

            var result = await this.context.Logbooks
                              .Include(bu => bu.BusinessUnit)
                              .Include(n => n.Notes)
                              .FirstOrDefaultAsync(x => x.Id == logbook.Id);

            return result.ToDTO();
        }

        public async Task<IReadOnlyCollection<LogbookDTO>> GetAllLogbooksByUserAsync(string userId)
        {
            var logbooksDTO = await this.context.Logbooks
                                       .Where(ul => ul.UsersLogbooks.Any(u => u.UserId == userId))
                                       .Include(bu => bu.BusinessUnit)
                                       .Include(n=>n.Notes)
                                       .Select(x => x.ToDTO())
                                       .ToListAsync();
            return logbooksDTO;
        }        
    }
}
