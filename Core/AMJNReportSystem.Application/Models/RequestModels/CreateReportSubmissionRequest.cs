namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportSubmissionRequest
    {
        public bool IsUpdate { get; set; }
        public Guid SubmissionWindowId { get; set; }
        public Guid reportSectionId { get; set; }
        public List<CreateReportResponseRequest> ReportResponses { get; set; } = new List<CreateReportResponseRequest>();
    }
}

