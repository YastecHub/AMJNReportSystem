namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class ReportSubmissionResponseModel
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ReportSubmissionName { get; set; }
        public Guid ReportTypeId { get; set; }
    }
}
