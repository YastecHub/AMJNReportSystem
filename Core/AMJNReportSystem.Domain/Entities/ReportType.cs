using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;
using System.Collections.ObjectModel;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportType : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public ICollection<ReportSection> ReportSections { get; set; } = new HashSet<ReportSection>();
    }
}
