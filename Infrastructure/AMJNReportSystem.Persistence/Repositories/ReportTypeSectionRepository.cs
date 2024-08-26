using AMJNReportSystem.Persistence.Context;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportTypeSectionRepository : IReportTypeSectionRepository
    {
        private readonly ApplicationContext _context;
        public ReportTypeSectionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReportTypeSection(ReportTypeSection reportTypeSection)
        {
            await _context.AddAsync(reportTypeSection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReportTypeSection(ReportTypeSection reportTypeSection)
        {
            _context.Update(reportTypeSection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<ReportTypeSection>> GetReportTypeSections(Expression<Func<ReportTypeSection, bool>> expression)
        {
            return await _context.ReportTypeSections.Where(expression).ToListAsync();
        }

        public async Task<ReportTypeSection> GetReportTypeSection(Expression<Func<ReportTypeSection, bool>> expression)
        {
            var result = await _context.ReportTypeSections.FirstOrDefaultAsync(expression);
            return result;
        }

        public async Task<ReportTypeSection> GetReportTypeSectionById(Guid id)
        {
            var result = await _context.ReportTypeSections.SingleOrDefaultAsync(r => r.Id == id);
            return result;
        }
    }
}
