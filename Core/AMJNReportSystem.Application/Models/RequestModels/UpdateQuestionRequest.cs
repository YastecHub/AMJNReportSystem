using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
	public class UpdateQuestionRequest
	{
		public string? LastModifiedBy { get; set; }
		public DateTime? LastModifiedOn { get; set; }
		public Guid ReportSectionId { get; set; }
		public string QuestionName { get; set; }
		public double Points { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>();
	}
}
