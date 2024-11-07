using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Interfaces;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportTypeRepository : IReportTypeRepository
    {
        private readonly ApplicationContext _context;
        private readonly ICurrentUser _currentUser;

        public ReportTypeRepository(ApplicationContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public List<ReportType> GetAllReportType()
        {
            return _context.ReportTypes.ToList();
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
            var reportTypes = await _context.ReportTypes
                 .Include(f => f.SubmissionWindows)
                .Where(f => f.Id == f.Id)
                .ToListAsync();
            return reportTypes;
        }

        public async Task<IList<ReportType>> GetQaidReports(string reportTypeName)
        {
            var reports = await _context.ReportTypes
                .Include(f => f.SubmissionWindows)
                .Where(r => r.Name == reportTypeName)
                .ToListAsync();

            return reports;
        }

      
        public async Task<ReportType?> GetReportTypeById(Guid id)
        {

            return await _context.ReportTypes.FindAsync(id);
        }

        public async Task<bool> UpdateReportType(ReportType reportType)
        {
            var report = _context.ReportTypes.Update(reportType);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<DashboardCountDto> DashBoardDataAsync()
        {
           var result = new DashboardCountDto();

            var jamaatId =  _currentUser.GetJamaatId();
            var circuitId = _currentUser.GetCircuit();

            result.TotalReportSubmittedForTheWholeMonth = await _context.ReportSubmissions.CountAsync();
            result.ReportSubmittedByCircuitCounts = await _context.ReportSubmissions.CountAsync(x => x.CircuitId == circuitId);
            result.ReportSubmittedByJamaatCounts = await _context.ReportSubmissions.CountAsync(x => x.JamaatId == jamaatId);

            return result;
        }
    }
}
