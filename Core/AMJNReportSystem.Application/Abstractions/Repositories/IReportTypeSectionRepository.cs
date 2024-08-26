using Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportTypeSectionRepository
    {

        Task<bool> CreateReportTypeSection(ReportTypeSection reportTypeSection);
        Task<bool> UpdateReportTypeSection(ReportTypeSection reportTypeSection);
        Task<ReportTypeSection> GetReportTypeSectionById(Guid id);
        Task<IList<ReportTypeSection>> GetReportTypeSections(Expression<Func<ReportTypeSection, bool>> expression);
        Task<ReportTypeSection> GetReportTypeSection(Expression<Func<ReportTypeSection, bool>> expression);
    }
}
