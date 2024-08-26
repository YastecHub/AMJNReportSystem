namespace AMJNReportSystem.Application.Models.DTOs
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Text { get; set; }
        public string ResponseType { get; set; }
        public double Points { get; set; }
    }
}
