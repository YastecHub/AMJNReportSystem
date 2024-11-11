namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportTypeQuestionDto
    {
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public List<ReportTypeSectionQuestion>? ReportTypeSectionQuestions { get; set; } = new List<ReportTypeSectionQuestion>();
    }
}
