using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateQuestionOption
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public object Text { get; set; }
    }
}
