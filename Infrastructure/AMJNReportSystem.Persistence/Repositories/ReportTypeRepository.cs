using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportTypeRepository : IReportTypeRepository
    {
        private readonly ApplicationContext _context;

        public ReportTypeRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<ReportType> AddReportType(ReportType reportType)
        {
            await _context.AddAsync(reportType);
            await _context.SaveChangesAsync();
            return reportType;
        }

        public async Task<bool> Exist(string reportTypeName)
        {
            var reportType = await _context.ReportTypes.AnyAsync(r => r.Title == reportTypeName);
            return reportType;
        }

        public async Task<IList<ReportType>> GetAllReportTypes()
        {
            var reportTypes = await _context.ReportTypes.ToListAsync();
            return reportTypes;
        }

        public async Task<IList<ReportType>> GetQaidReports()
        {
            var report = await _context.ReportTypes.Where(r => r.Title == "").ToListAsync();
            return report;
        }

        public async Task<ReportType> GetReportTypeById(Guid id)
        {
            var reportType = await _context.ReportTypes.SingleOrDefaultAsync(x => x.Id == id);
            return reportType;
        }

        public async Task<ReportType> UpdateReportType(ReportType reportType)
        {
            _context.Update(reportType);
            _context.SaveChanges();
            return reportType;
        }
    }
}
