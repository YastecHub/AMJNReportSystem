using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportSubmission : AuditableEntity
    {
        public Guid JamaatId { get; set; }
        public Guid ReportTypeId { get; set; }
        public string JammatEmailAddress { get; set; }
        public ReportType ReportType { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag ReportTag { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public List<ReportResponse> Answers { get; set; } = new List<ReportResponse>();
    }
}
