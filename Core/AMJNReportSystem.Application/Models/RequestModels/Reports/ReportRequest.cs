namespace AMJNReportSystem.Application.Models.RequestModels.Reports
{
    public class ReportRequest
    {
        public Guid ReporterId { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public IList<SectionReportRequest> SectionReports { get; set; }
    }

    public class SectionReportRequest
    {
        public Guid ReportTypeSectionId { get; set; }
        public string ReportTypeReportSectionName { get; set; } = null!;
        public IList<SectionDataRequest> SectionData { get; set; }
    }

    public class SectionDataRequest
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string? QuestionResponse { get; set; }
    }
}
