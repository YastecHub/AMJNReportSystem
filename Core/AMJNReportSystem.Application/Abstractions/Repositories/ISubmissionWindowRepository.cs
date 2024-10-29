using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface ISubmissionWindowRepository
    {
        Task<bool> AddSubmissionWindow(SubmissionWindow submissionWindow);
        Task<bool> UpdateSubmissionWindow(SubmissionWindow submissionWindow);
        Task<SubmissionWindow> GetSubmissionWindowsById(Guid id);
        Task<IList<SubmissionWindow>> GetAllSubmissionWindowsAsync(Expression<Func<SubmissionWindow, bool>> expression);
        Task<SubmissionWindow> GetActiveSubmissionWindows(Guid id);
        Task<bool> GetReportTypeExist(Guid reportTypeId);
        Task<ReportSubmission?> CheckIfReportHasBeenSubmittedByJammatPresident(Guid submissionWindowId, int JamaatId);

        Task<bool?> DeleteReportSubmissionAnswer(List<ReportResponse> answers);
        Task<ReportSubmission?> CheckIfReportSectionHasBeenSubmitted(Guid submissionWindowId, int JamaatId, Guid reportSectionId);
    }
}
