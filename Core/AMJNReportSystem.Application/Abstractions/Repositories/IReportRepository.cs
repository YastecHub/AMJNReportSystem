using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportRepository
    {
        Task<Report> AddAsync(Report report);
        Task<Report> UpdateAsync(Report report);
        Task<IReadOnlyList<Report>> GetSelectedReportsByReportSubmission(Guid reportSubmissionId, IList<Guid> reportIds);
        Task<PaginatedResult<Report>> GetReportsByReportSubmission(Guid reportSubmssionId, PaginationFilter filter);
        Task<PaginatedResult<Report>> GetReportsByReporter(Guid reporterId, PaginationFilter filter);
        Task<PaginatedResult<Report>> GetReportsByReportSubmissionAndReporter(Guid reportSubmissionId, Guid reporterId, PaginationFilter filter);
        Task<PaginatedResult<Report>> GetSectionReportsByReportSubmission(Guid reportSubmissionId, Guid sectionId, PaginationFilter filter);
    }
}
