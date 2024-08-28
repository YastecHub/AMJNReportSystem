using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportSubmissionRepository
    {
        Task<ReportSubmission> AddReportSubmissionAsync(ReportSubmission reportSubmission);
        Task<bool> Exist(string reportSubmissionName);
        Task<PaginatedResult<ReportSubmissionResponseModel>> GetReportTypeSubmissionsAsync(PaginationFilter filter);
        Task<ReportSubmission> GetReportTypeSubmissionAsync(Expression<Func<ReportSubmission, bool>> expression);
        Task<ReportSubmission> UpdateReportSubmission(ReportSubmission reportSubmission);
    }
}
