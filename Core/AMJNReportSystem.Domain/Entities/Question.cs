using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class Question : AuditableEntity
    {
        public string QuestionName { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseType ResponseType { get; set; }
        public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>(); 
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public Guid SectionId { get; set; } 
        public ReportSection ReportSection { get; set; }
    }
}
