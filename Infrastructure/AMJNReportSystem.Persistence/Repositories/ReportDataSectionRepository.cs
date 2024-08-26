using AMJNReportSystem.Persistence.Context;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportDataSectionRepository : IReportDataSectionRepository
    {

        private readonly ApplicationContext _context;

        public ReportDataSectionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ReportDataSection> AddAsync(ReportDataSection reportDataSection)
        {
            await _context.ReportDataSections.AddAsync(reportDataSection);
            await _context.SaveChangesAsync();
            return reportDataSection;
        }

        public async Task<ReportDataSection> GetReportDataSection(Expression<Func<ReportDataSection, bool>> expression)
        {
            var reportDataSection = await _context.ReportDataSections.FirstOrDefaultAsync(expression);
            return reportDataSection;
        }

        public async Task<ReportDataSection> UpdateAsync(ReportDataSection reportDataSection)
        {
            _context.ReportDataSections.Update(reportDataSection);
            await _context.SaveChangesAsync();
            return reportDataSection;
        }
    }
}
