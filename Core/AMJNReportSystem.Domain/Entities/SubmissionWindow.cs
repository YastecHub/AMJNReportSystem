using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class SubmissionWindow : AuditableEntity
    {
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public Guid ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; }
        public SubmissionOption SubmissionOption { get; set; }
    }
}
