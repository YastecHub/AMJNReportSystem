using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportSubmission : AuditableEntity
    {
        public Guid ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public ICollection<SubmissionWindow> SubmissionWindows { get; set; } = new HashSet<SubmissionWindow>();
    }
}
