using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportTypeRepository : IReportTypeRepository
    {
        private readonly ApplicationContext _context;

        public ReportTypeRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<ReportType> CreateReportType(ReportType reportType)
        {
            var report = await _context.AddAsync(reportType);
            await _context.SaveChangesAsync();
            return reportType;
        }
        public async Task<bool> Exist(string reportTypeName)
        {
            var reportType = await _context.ReportTypes.AnyAsync(r => r.Name == reportTypeName);
            return reportType;
        }


        public async Task<IList<ReportType>> GetAllReportTypes()
        {
            var reportTypes = await _context.ReportTypes.Where(f => f.Id == f.Id).ToListAsync();
            return reportTypes;
        }

        public async Task<IList<ReportType>> GetQaidReports(string reportTypeName)
        {
            var reports = await _context.ReportTypes
                .Where(r => r.Title == reportTypeName)
                .ToListAsync();

            return reports;
        }

      
        public async Task<ReportType> GetReportTypeById(Guid id)
        {

            return await _context.ReportTypes.FindAsync(id);
        }

        public async Task<ReportType> UpdateReportType(ReportType reportType)
        {
            var report = _context.ReportTypes.Update(reportType);
            await _context.SaveChangesAsync();
            return reportType;
        }


    }
}
