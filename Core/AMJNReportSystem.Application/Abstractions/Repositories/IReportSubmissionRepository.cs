using AMJNReportSystem.Application.Models;
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
        Task<List<ReportSubmission>> GetReportSubmissionsByReportTypeAsync(Guid reportTypeId);
        Task<List<ReportSubmission>> GetReportSubmissionsByCircuitIdAsync(int circuitId);
        Task<List<ReportSubmission>> GetReportSubmissionsByJamaatIdAsync(int jamaatId);

        List<ReportSubmission> GetAllReportSubmission();
    }
}
