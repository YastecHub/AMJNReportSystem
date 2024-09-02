using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportType : AuditableEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public ReportTag ReportTag { get; set; }
        public bool IsActive { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
