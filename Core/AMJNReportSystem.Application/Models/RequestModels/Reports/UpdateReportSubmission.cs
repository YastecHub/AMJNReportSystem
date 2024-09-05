using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels.Reports
{
    public class UpdateReportSubmission
    {
        public string JammatEmailAddress { get; set; }
        public ReportType ReportType { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag ReportTag { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public string LastModifiedBy { get; set; }
        public List<ReportResponseDto> Answers { get; set; } = new List<ReportResponseDto>();
    }
}
