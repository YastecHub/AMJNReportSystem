namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
