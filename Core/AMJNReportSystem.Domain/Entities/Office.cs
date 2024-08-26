using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class Office : AuditableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}
