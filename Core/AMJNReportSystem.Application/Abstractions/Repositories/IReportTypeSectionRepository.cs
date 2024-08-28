using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportTypeSectionRepository
    {

        Task<bool> CreateReportTypeSection(ReportSection reportTypeSection);
        Task<bool> UpdateReportTypeSection(ReportSection reportTypeSection);
        Task<ReportSection> GetReportTypeSectionById(Guid id);
        Task<IList<ReportSection>> GetReportTypeSections(Expression<Func<ReportSection, bool>> expression);
        Task<ReportSection> GetReportTypeSection(Expression<Func<ReportSection, bool>> expression);
    }
}
