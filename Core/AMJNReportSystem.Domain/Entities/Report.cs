using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class Report : AuditableEntity
    {
        public Guid ReporterId { get; set; }
        public Reporter Reporter { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public ReportSubmissionTimeliness ReportSubmissionTimeliness { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
    }

}