using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportTypeRepository
    {
        Task<ReportType> CreateReportType(ReportType reportType);
        Task<IList<ReportType>> GetAllReportTypes();
        Task<IList<ReportType>> GetQaidReports(string reportTypeName);
        Task<ReportType> GetReportTypeById(Guid id);
        Task<ReportType> UpdateReportType(ReportType reportType);
        Task<bool> Exist(string reportTypeName);
    }
}