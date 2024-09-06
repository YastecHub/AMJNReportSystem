using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
	public class CreateQuestionRequest
	{
		public Guid ReportSectionId { get; set; }
		public string QuestionName { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<CreateQuestionOptionRequest>? Options { get; set; } = new List<CreateQuestionOptionRequest>();
	}
}
