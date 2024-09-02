using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface ISubmissionWindowRepository
    {
        Task<bool> AddSubmissionWindow(SubmissionWindow submissionWindow);
        Task<bool> UpdateSubmissionWindow(SubmissionWindow submissionWindow);
        Task<SubmissionWindow> GetSubmissionWindowAsync(Expression<Func<SubmissionWindow, bool>> expression);
        Task<IEnumerable<SubmissionWindow>> GetAllSubmissionWindowsAsync(Guid? reportTypeId, int? month, int? year, bool? isLocked, DateTime? startDate, DateTime? endDate);
    }
}
