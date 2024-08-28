using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportDataSectionRepository
    {
        Task<ReportSection> AddAsync(ReportSection reportSection);
        Task<ReportSection> UpdateAsync(ReportSection reportSection);
        Task<ReportSection> GetReportDataSection(Expression<Func<ReportSection, bool>> expression);
    }
}
