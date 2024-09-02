using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportResponse : AuditableEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public object? TextAnswer { get; set; } 
        public Guid? QuestionOptionId { get; set; }
        public QuestionOption? QuestionOption { get; set; }
        public string? Report { get; set; }
    }
}
