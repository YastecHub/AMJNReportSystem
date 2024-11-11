using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;
namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportSubmissionDto
    {
        public int JamaatId { get; set; }
        public int CircuitId { get; set; }
        public Guid ReportTypeId { get; set; }
        public string JammatEmailAddress { get; set; } 
        public string SubmissionWindowName { get; set; }
        public ReportType ReportType { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag? ReportTag { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public SubmissionWindow SubmissionWindow { get; set; }
        public List<ReportResponseDto> Answers { get; set; } = new List<ReportResponseDto>();

    }
}