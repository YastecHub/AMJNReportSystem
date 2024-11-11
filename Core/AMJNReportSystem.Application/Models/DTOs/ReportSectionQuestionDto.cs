using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportSectionQuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseType ResponseType { get; set; }
        public List<ReportQuestionOptionDto>? Options { get; set; } = new List<ReportQuestionOptionDto>();
    }
}
