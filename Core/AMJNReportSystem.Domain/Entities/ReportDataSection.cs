using AMJNReportSystem.Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportDataSection : AuditableEntity
    {
        public Guid ReportId { get; set; }
        public Guid ReportTypeSectionId { get; set; }
        public string ReportSectionName { get; set; } = default!;
        public Report Report { get; set; }
        public string Data { get; set; } = default!;
    }
}
