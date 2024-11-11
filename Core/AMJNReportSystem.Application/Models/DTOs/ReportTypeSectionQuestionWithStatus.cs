namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportTypeSectionQuestionWithStatus
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
