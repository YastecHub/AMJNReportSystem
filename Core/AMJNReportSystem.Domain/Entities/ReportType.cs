using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportType : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ReportTag ReportTag { get; set; }
        public bool isActive { get; set; }
        public ICollection<ReportTypeSection> Sections { get; set; } = new HashSet<ReportTypeSection>();
    }

}
