using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ReportTag ReportTag { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}
