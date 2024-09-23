namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportResponseRequest
    {
        public Guid QuestionId { get; set; }
        public string TextAnswer { get; set; }
        public Guid? QuestionOptionId { get; set; }
        public string Report { get; set; }
    }
}
