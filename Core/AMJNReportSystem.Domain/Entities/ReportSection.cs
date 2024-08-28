using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportSection : AuditableEntity
    {
        public string SectionName { get; set; }
        public int SectionValue { get; set; }

    }
}
