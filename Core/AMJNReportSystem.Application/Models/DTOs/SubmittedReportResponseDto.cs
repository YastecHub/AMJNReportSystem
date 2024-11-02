using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class SubmittedReportResponseDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string TextAnswer { get; set; }
        public Guid? QuestionOptionId { get; set; }
        public string? Report { get; set; }
        public string QuestionName { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseType ResponseType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public Guid ReportSectionId { get; set; }
        public string ReportSectionName { get; set; }

        public List<SubmittedQuestionOption> SubmittedQuestionOptions { get; set; }
    }
}
