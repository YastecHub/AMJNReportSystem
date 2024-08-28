using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Domain.Entities;
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

        public async Task<bool> CreateReportTypeSection(ReportSection reportTypeSection)
        {
            await _context.AddAsync(reportTypeSection);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<ReportSection> GetReportTypeSection(Expression<Func<ReportSection, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSection> GetReportTypeSectionById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ReportSection>> GetReportTypeSections(Expression<Func<ReportSection, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateReportTypeSection(ReportSection reportTypeSection)
        {
            _context.Update(reportTypeSection);
            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
