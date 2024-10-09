using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using System.Linq.Expressions;
using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportSectionRepository : IReportSectionRepository
    {
        private readonly ApplicationContext _context;
        public ReportSectionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReportSection(ReportSection reportSection)
        {
            await _context.ReportSections.AddAsync(reportSection);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ReportSection?> GetReportSection(Expression<Func<ReportSection, bool>> expression)
        {
            return await _context.ReportSections.FirstOrDefaultAsync(expression);
        }

        public async Task<ReportSection?> GetReportSectionById(Guid id)
        {
            return await _context.ReportSections.FindAsync(id);
        }

        public async Task<IList<ReportSection>> GetReportSections(Expression<Func<ReportSection, bool>> expression)
        {
            return await _context.ReportSections
                .Include(x => x.ReportType)
                .Where(expression)
                .OrderBy(x => x.ReportSectionValue)
                .ToListAsync();
        }

        public async Task<bool> UpdateReportSection(ReportSection reportSection)
        {
            _context.ReportSections.Update(reportSection);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ReportTypeExistsAsync(Guid reportTypeId)
        {
            return await _context.ReportTypes.AnyAsync(rt => rt.Id == reportTypeId);
        }

        public List<ReportSection> GetAllReportSection()
        {
            return _context.ReportSections.ToList();
        }
    }
}

