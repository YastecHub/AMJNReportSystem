using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportTypeSection : AuditableEntity
    {
        public Guid ReportTypeId { get; set; }
        //public ReportType ReportType { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; } = null!;
        //public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public bool isActive { get; set; }
    }
}
