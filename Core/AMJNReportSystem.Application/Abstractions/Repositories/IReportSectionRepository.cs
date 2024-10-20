using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportSectionRepository
    {

        Task<bool> CreateReportSection(ReportSection reportSection);
        Task<bool> UpdateReportSection(ReportSection reportSection);
        Task<ReportSection?> GetReportSectionById(Guid id);
        Task<IList<ReportSection>> GetReportSections(Expression<Func<ReportSection, bool>> expression);
        Task<ReportSection> GetReportSection(Expression<Func<ReportSection, bool>> expression);
        Task<bool> ReportTypeExistsAsync(Guid reportTypeId);
        List<ReportSection> GetAllReportSection();
        Task<bool> ReportSectionExist(string reportSectionName, int reportSectionValue);
    }
}
