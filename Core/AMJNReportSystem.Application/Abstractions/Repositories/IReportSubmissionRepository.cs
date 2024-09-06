using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportSubmissionRepository
    {
        Task<ReportSubmission> CreateReportSubmissionAsync(ReportSubmission reportSubmission);
        Task<bool> Exist(string reportSubmissionName);
        Task<ReportSubmission> GetReportTypeSubmissionByIdAsync(Guid id);
        Task<ReportSubmission> UpdateReportSubmission(ReportSubmission reportSubmission);
        Task<PaginatedResult<ReportSubmission>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter);
    }
}
