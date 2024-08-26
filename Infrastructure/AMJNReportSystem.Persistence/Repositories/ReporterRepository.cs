using AMJNReportSystem.Persistence.Context;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReporterRepository : IReporterRepository
    {
        private readonly ApplicationContext _context;

        public ReporterRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Reporter> AddReporter(Reporter reporter)
        {
            await _context.AddAsync(reporter);
            await _context.SaveChangesAsync();
            return reporter;
        }

        public async Task<Reporter> GetReporterById(Guid reporterId)
        {
            var reporter = await _context.Reporters.SingleOrDefaultAsync(x => x.Id == reporterId);
            return reporter;
        }
    }
}
