using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class ReportSubmissionResponseDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ReportSubmissionName { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid JamaatId { get; set; }
        public string JammatEmailAddress { get; set; }
        public ReportType ReportType { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag ReportTag { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public List<ReportResponse> Answers { get; set; } = new List<ReportResponse>();
    }
}
