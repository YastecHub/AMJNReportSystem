namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportTypeSectionQuestion
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }

        public List<ReportSectionQuestionDto>? ReportSectionQuestions { get; set; } = new List<ReportSectionQuestionDto>();
    }
}
