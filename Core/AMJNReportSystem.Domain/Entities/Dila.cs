using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class Dila : AuditableEntity
    {
        public Guid ZoneId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
