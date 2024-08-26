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
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationContext _context;

        public SectionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ReportTypeSection> AddSection(ReportTypeSection section)
        {
            await _context.AddAsync(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<IList<ReportTypeSection>> GetSectionsByReportType(Guid reportTypeId)
        {
            var sections = await _context.ReportTypeSections.Where(x => x.ReportTypeId == reportTypeId).ToListAsync();
            return sections;
        }
    }
}
