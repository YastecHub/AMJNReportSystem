using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportSubmission : AuditableEntity
    {
        public int JamaatId { get; set; }
        public int CircuitId { get; set; }
        public string JammatEmailAddress { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag? ReportTag { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public Guid ReportSectionId { get; set; }
        public ReportSection ReportSection { get; set; }
        public ICollection<ReportResponse> Answers { get; set; } = new HashSet<ReportResponse>();

    }
}
