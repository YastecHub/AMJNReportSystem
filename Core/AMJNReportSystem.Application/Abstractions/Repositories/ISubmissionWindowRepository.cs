using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface ISubmissionWindowRepository
    {
        Task<bool> AddSubmissionWindow(SubmissionWindow submissionWindow);
        Task<SubmissionWindow> UpdateSubmissionWindow(SubmissionWindow submissionWindow);
        Task<SubmissionWindow> GetSubmissionWindowAsync(Expression<Func<SubmissionWindow, bool>> expression);
        Task<IEnumerable<SubmissionWindow>> GetAllSubmissionWindowsAsync(Guid? reportTypeId, int? month, int? year, bool? isLocked, DateTime? startDate, DateTime? endDate);
    }
}
