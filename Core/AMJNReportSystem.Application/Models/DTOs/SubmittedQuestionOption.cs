namespace AMJNReportSystem.Domain.Entities
{
    public class SubmittedQuestionOption 
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
    }
}
