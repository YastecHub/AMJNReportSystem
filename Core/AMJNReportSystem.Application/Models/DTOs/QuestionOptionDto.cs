namespace AMJNReportSystem.Application.Models.DTOs
{
    public class QuestionOptionDto 
	{
		public Guid Id { get; set; }
		public Guid QuestionId { get; set; }
		public string? QuestionName { get; set; }
		public string OptionText { get; set; }
	}
}
