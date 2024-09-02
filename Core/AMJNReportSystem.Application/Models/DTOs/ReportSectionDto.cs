namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportSectionDto
    {
        public Guid Id { get; set; }
        public string ReportSectionName { get; set; }
        public int ReportSectionValue { get; set; }
        public string Description { get; set; }
        public Guid ReportTypeId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
