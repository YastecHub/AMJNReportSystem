using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportResponseDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string TextAnswer { get; set; } 
        public Guid? QuestionOptionId { get; set; }
        public string? Report { get; set; }
    }

    public class CreateReportResponseRequest
    {
        public Guid QuestionId { get; set; }
        public string TextAnswer { get; set; }
        public Guid? QuestionOptionId { get; set; }
        public string Report { get; set; }
    }

    public class UpdateReportResponseRequest
    {
        public string TextAnswer { get; set; }
        public string Report { get; set; }
    }
}
