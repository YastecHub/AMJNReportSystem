using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.DTOs
{
	public class QuestionDto
	{
		public Guid Id { get; set; }
		public Guid ReportSectionId { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime CreatedOn { get; private set; }
		public string? LastModifiedBy { get; set; }
		public DateTime? LastModifiedOn { get; set; }
		public DateTime? DeletedOn { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
        public string SectionName { get; set; } 
        public string QuestionName { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>();
	}

}
