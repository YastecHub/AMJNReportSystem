using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface ISectionRepository
    {
        Task<ReportTypeSection> AddSection(ReportTypeSection section);
        Task<IList<ReportTypeSection>> GetSectionsByReportType(Guid reportTypeId);
    }
}
