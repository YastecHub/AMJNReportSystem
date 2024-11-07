using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportTypeRepository
    {
        Task<ReportType> CreateReportType(ReportType reportType);
        Task<IList<ReportType>> GetAllReportTypes();
        Task<IList<ReportType>> GetQaidReports(string reportTypeName);
        Task<ReportType> GetReportTypeById(Guid id);
        Task<bool> UpdateReportType(ReportType reportType);
        Task<bool> Exist(string reportTypeName);
        List<ReportType> GetAllReportType();
        Task<DashboardCountDto> DashBoardDataAsync(int jamaatId, int circuitId);

    }
}