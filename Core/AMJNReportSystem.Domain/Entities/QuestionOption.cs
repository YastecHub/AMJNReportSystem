using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class QuestionOption : AuditableEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public object Text { get; set; }
    }
}
