using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class ReportSubmissionResponseDto
    {
        public int JamaatId { get; set; }
        public int CircuitId { get; set; }
        public string JammatEmailAddress { get; set; }
        public string ReportTypeName { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag ReportTag { get; set; }
        public int  SubmissionWindowYear { get; set; }
        public int SubmissionWindowMonth { get; set; } 
        public List<ReportResponseDto> Answers { get; set; } = new List<ReportResponseDto>(); 
    }
}
