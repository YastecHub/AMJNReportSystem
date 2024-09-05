using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
	public class CreateQuestionRequest
	{
		public string? CreatedBy { get; set; }
		public DateTime CreatedOn { get; private set; }
		public Guid ReportSectionId { get; set; }
		public string QuestionName { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>();
	}
}
