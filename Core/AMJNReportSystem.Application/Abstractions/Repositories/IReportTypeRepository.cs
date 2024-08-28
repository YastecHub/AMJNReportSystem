using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportTypeRepository
    {
        Task<ReportType> AddReportType(ReportType reportType);
        Task<ReportType> GetReportTypeById(Guid id);
        Task<IList<ReportType>> GetAllReportTypes();
        public Task<IList<ReportType>> GetQaidReports();
        Task<ReportType> UpdateReportType(ReportType reportType);
        Task<bool> Exist(string reportTypeName);
    }
}