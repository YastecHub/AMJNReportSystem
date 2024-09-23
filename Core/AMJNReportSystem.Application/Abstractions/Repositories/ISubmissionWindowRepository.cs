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
    }
}
