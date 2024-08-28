using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Method for submitting report 
        /// </summary>
        Task<Result<bool>> SaveReport(ReportRequest request);
    }
}
