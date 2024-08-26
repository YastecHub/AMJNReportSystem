using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReportDataSectionRepository
    {
        Task<ReportDataSection> AddAsync(ReportDataSection reportDataSection);
        Task<ReportDataSection> UpdateAsync(ReportDataSection reportDataSection);
        Task<ReportDataSection> GetReportDataSection(Expression<Func<ReportDataSection, bool>> expression);
    }
}
