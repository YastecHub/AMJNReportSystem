using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class Zone : AuditableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
