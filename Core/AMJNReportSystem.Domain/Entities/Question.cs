using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class Question : AuditableEntity
    {
        public string QuestionName { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseType ResponseType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public Guid ReportSectionId { get; set; } 
        public ReportSection ReportSection { get; set; }

        public ICollection<QuestionOption>? Options { get; set; } = new HashSet<QuestionOption>();
    }
}
