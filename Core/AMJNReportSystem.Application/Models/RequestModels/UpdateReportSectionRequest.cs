namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class UpdateReportSectionRequest
    {
        public string ReportSectionName { get; set; } = null!;  
        public int ReportSectionValue { get; set; }  
        public string Description { get; set; } = null!;
        public Guid ReportTypeId { get; set; }
    }
}