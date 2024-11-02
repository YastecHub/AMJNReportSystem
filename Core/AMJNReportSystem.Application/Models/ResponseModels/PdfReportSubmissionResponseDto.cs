using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class PdfReportSubmissionResponseDto
    {
        public int JamaatId { get; set; }
        public string? JamaatName { get; set; }
        public int CircuitId { get; set; }
        public string? CircuitName { get; set; }
        public string? JammatEmailAddress { get; set; }
        public string? ReportTypeName { get; set; }
        public ReportSubmissionStatus? ReportSubmissionStatus { get; set; }
        public ReportTag? ReportTag { get; set; }
        public int SubmissionWindowYear { get; set; }
        public int SubmissionWindowMonth { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Status { get; set; }
        public int NumberOfQuestion { get; set; }
        public int NumberOfQuestionSections { get; set; }
        public List<ReportResponseDto>? Answers { get; set; } = new List<ReportResponseDto>();
    }
}
