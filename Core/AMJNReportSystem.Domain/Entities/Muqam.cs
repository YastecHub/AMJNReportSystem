using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class Muqam : AuditableEntity
    {
        public Guid DilaId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
